using Microsoft.EntityFrameworkCore;
using Fortinite_Project.Web.Data;
using Fortinite_Project.Web.Models;
using Fortinite_Project.Web.DTOs;

namespace Fortinite_Project.Web.Services;

public class FortniteSyncService : IFortniteSyncService
{
    private readonly AppDbContext _dbContext;
    private readonly HttpClient _httpClient;
    private readonly ILogger<FortniteSyncService> _logger;

    private readonly List<string> _urlsApi = new()
    {
        "https://fortnite-api.com/v2/cosmetics/br?language=pt-BR",
        "https://fortnite-api.com/v2/cosmetics/cars?language=pt-BR",
        "https://fortnite-api.com/v2/cosmetics/tracks?language=pt-BR",
        "https://fortnite-api.com/v2/cosmetics/instruments?language=pt-BR",
        "https://fortnite-api.com/v2/cosmetics/lego?language=pt-BR",
        "https://fortnite-api.com/v2/cosmetics/beans?language=pt-BR",
        "https://fortnite-api.com/v2/cosmetics/lego/kits?language=pt-BR"
    };

    public FortniteSyncService(AppDbContext context, HttpClient httpClient, ILogger<FortniteSyncService> logger)
    {
        _dbContext = context;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task RunInitialSyncAsync()
    {
        var count = await _dbContext.Cosmeticos.CountAsync();
        if (count > 0)
        {
            _logger.LogInformation("Banco já populado ({Count} itens). Atualizando loja...", count);
            await SyncShopAndNewStatusAsync();
            return;
        }

        await SyncAllBaseCosmeticsAsync();
    }

    public async Task SyncAllBaseCosmeticsAsync()
    {
        // Cache de IDs em memória para performance extrema
        var existingIds = await _dbContext.Cosmeticos.Select(c => c.Id).ToHashSetAsync();

        foreach (var url in _urlsApi)
        {
            try 
            {
                _logger.LogInformation($"Sincronizando: {url}");
                var response = await _httpClient.GetFromJsonAsync<FortniteApiResponse_DTO>(url);
                
                if (response?.Data == null || response.Data.Count == 0)
                {
                    _logger.LogWarning($"Nenhum dado retornado ou mapeado da URL {url}. Verifique os DTOs.");
                    continue;
                }

                int novosItens = 0;
                foreach (var dto in response.Data)
                {
                    if (!existingIds.Contains(dto.Id))
                    {
                        var novo = new Cosmetico 
                        { 
                            Id = dto.Id,
                            
                            Nome = dto.Nome ?? "Sem Nome",
                            
                            Descricao = dto.Descricao ?? "",
                            
                            UrlImagem = dto.Images?.Small ?? dto.Images?.Large ?? dto.Images?.Icon ?? "",
                            
                            Tipo = dto.TypeInfo?.DisplayValue ?? dto.TypeInfo?.Value ?? "Desconhecido",
                            
                            Raridade = dto.Rarity?.DisplayValue ?? dto.Series?.Value ?? "Comum",
                            
                            Preco = dto.Preco,
                            dataInclusao = dto.dataInclusao,
                            isForSale = false 
                        };
                        
                        await _dbContext.Cosmeticos.AddAsync(novo);
                        existingIds.Add(dto.Id);
                        novosItens++;
                    }
                }

                if (novosItens > 0)
                {
                    await _dbContext.SaveChangesAsync();
                    _logger.LogInformation($"Sucesso: {novosItens} novos itens adicionados da URL {url}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro crítico na URL {url}: {ex.Message}");
            }
        }
    }

    public async Task SyncShopAndNewStatusAsync()
    {
        _logger.LogInformation("Iniciando sincronização da Loja Diária...");
        
        // 1. Reseta tudo para não estar à venda
        await _dbContext.Database.ExecuteSqlRawAsync("UPDATE Cosmeticos SET isForSale = 0");

        try
        {
            var shopResponse = await _httpClient.GetFromJsonAsync<FortniteShopResponse_DTO>("https://fortnite-api.com/v2/shop?language=pt-BR");
            
            if (shopResponse?.Data?.Entries == null)
            {
                _logger.LogWarning("Loja retornou vazia ou estrutura mudou.");
                return;
            }
            
            int atualizados = 0;
            HashSet<string> idsNaLoja = new();

            foreach (var entry in shopResponse.Data.Entries)
            {
                var itensDaEntrada = (entry.Items ?? new List<CosmeticoApi_DTO>())
                                    .Concat(entry.BrItems ?? new List<CosmeticoApi_DTO>());

                foreach (var itemApi in itensDaEntrada)
                {
                    if (!idsNaLoja.Contains(itemApi.Id))
                    {
                        var cosmetico = await _dbContext.Cosmeticos.FindAsync(itemApi.Id);
                        if (cosmetico != null)
                        {
                            cosmetico.isForSale = true;
                            cosmetico.Preco = entry.FinalPrice;
                            idsNaLoja.Add(itemApi.Id);
                            atualizados++;
                        }
                    }
                }
            }
            
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Loja atualizada: {atualizados} itens marcados como à venda.");

        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao sincronizar loja: {ex.Message}");
        }
    }
}