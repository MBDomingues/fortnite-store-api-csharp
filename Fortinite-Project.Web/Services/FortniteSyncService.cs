using Microsoft.EntityFrameworkCore;
using Fortinite_Project.Web.Data;
using Fortinite_Project.Web.Models;
using Microsoft.Extensions.Logging;
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
            _logger.LogInformation("Banco já populado. Atualizando loja...");
            await SyncShopAndNewStatusAsync();
            return;
        }

        await SyncAllBaseCosmeticsAsync();
    }

    public async Task SyncAllBaseCosmeticsAsync()
    {
        foreach (var url in _urlsApi)
        {
            try 
            {
                var response = await _httpClient.GetFromJsonAsync<FortniteApiResponse_DTO>(url);
                if (response?.Data == null)
                {
                    _logger.LogWarning($"Nenhum dado retornado da URL {url}");
                    continue;
                }

                foreach (var dto in response.Data)
                {
                    await SaveOrUpdateCosmeticoAsync(dto);
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro na URL {url}: {ex.Message}");
            }
        }
    }

    private async Task SaveOrUpdateCosmeticoAsync(CosmeticoApi_DTO dto)
    {
        var existing = await _dbContext.Cosmeticos.FindAsync(dto.Id);
        var cosmetico = existing ?? new Cosmetico { Id = dto.Id };

        cosmetico.Nome = dto.Nome;
        cosmetico.Descricao = dto.Descricao;
        cosmetico.UrlImagem = dto.UrlImagem;
        cosmetico.Tipo = dto.Tipo;
        cosmetico.Raridade = dto.Raridade;
        cosmetico.Preco = dto.Preco;
        cosmetico.isForSale = dto.isForSale;
        cosmetico.dataInclusao = dto.dataInclusao;
        cosmetico.isBundle = dto.isBundle;
        cosmetico.bundleItemsJson = dto.bundleItemsJson;
        cosmetico.coresJson = dto.coresJson;

        if (existing == null)
        {
            await _dbContext.Cosmeticos.AddAsync(cosmetico);
        }
        else
        {
            _dbContext.Cosmeticos.Update(cosmetico);
        }
    }


    public async Task SyncShopAndNewStatusAsync()
    {
        await _dbContext.Database.ExecuteSqlRawAsync("UPDATE Cosmeticos SET IsForSale = 0, IsNew = 0");
        try
        {
            var shopResponse = await _httpClient.GetFromJsonAsync<FortniteShopResponse_DTO>("https://fortnite-api.com/v2/shop?language=pt-BR");
            
            if (shopResponse?.Data == null)
            {
                _logger.LogWarning("Nenhum dado retornado da loja.");
                return;
            }
            
            foreach (var shopItem in shopResponse.Data)
            {
                foreach (var featured in shopItem.Featured)
                {
                    var cosmetico = await _dbContext.Cosmeticos.FindAsync(featured.Id);
                    if (cosmetico != null)
                    {
                        cosmetico.isForSale = true;
                        _dbContext.Cosmeticos.Update(cosmetico);
                    }
                }
            }
            await _dbContext.SaveChangesAsync();

        }catch (Exception ex)
        {
            _logger.LogError($"Erro ao sincronizar loja: {ex.Message}");
        }
    }
}
