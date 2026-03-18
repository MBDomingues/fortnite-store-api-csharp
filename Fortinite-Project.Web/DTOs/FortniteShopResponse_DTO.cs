using System.Text.Json.Serialization;

namespace Fortinite_Project.Web.DTOs;

public class FortniteShopResponse_DTO
{
    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("data")]
    public ShopData_DTO? Data { get; set; }
}

public class ShopData_DTO
{
    [JsonPropertyName("hash")]
    public string? Hash { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("entries")]
    public List<ShopEntry_DTO>? Entries { get; set; }
}

public class ShopEntry_DTO
{
    [JsonPropertyName("regularPrice")]
    public int RegularPrice { get; set; }

    [JsonPropertyName("finalPrice")]
    public int FinalPrice { get; set; }

    [JsonPropertyName("items")]
    public List<CosmeticoApi_DTO>? Items { get; set; }
    
    [JsonPropertyName("brItems")]
    public List<CosmeticoApi_DTO>? BrItems { get; set; }
}