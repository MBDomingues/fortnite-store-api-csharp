using System.Text.Json.Serialization;

namespace Fortinite_Project.Web.DTOs;

public class FortniteShopResponse_DTO
{
    [JsonPropertyName("data")]
    public List<ShopItem_DTO>? Data { get; set; }
}

