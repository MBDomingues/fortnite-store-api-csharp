using System.Text.Json.Serialization;

namespace Fortinite_Project.Web.DTOs;

public class FortniteShopResponse_DTO
{
    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("data")]
    public ShopData_DTO? Data { get; set; } // Agora é um Objeto, não uma Lista
}

public class ShopData_DTO
{
    [JsonPropertyName("hash")]
    public string? Hash { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("entries")]
    public List<ShopEntry_DTO>? Entries { get; set; } // A lista está aqui dentro
}

public class ShopEntry_DTO
{
    [JsonPropertyName("regularPrice")]
    public int RegularPrice { get; set; }

    [JsonPropertyName("finalPrice")]
    public int FinalPrice { get; set; }

    // Cada entrada da loja pode ter vários itens (ex: skin + mochila)
    [JsonPropertyName("items")]
    public List<CosmeticoApi_DTO>? Items { get; set; }
    
    // As vezes vem como brItems, mas geralmente items cobre a maioria
    [JsonPropertyName("brItems")]
    public List<CosmeticoApi_DTO>? BrItems { get; set; }
}