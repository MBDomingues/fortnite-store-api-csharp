using System.Text.Json.Serialization;
namespace Fortinite_Project.Web.DTOs;

public class ShopItem_DTO
{   
    [JsonPropertyName("granted")]
    public List<CosmeticoApi_DTO>? Featured { get; set; }

    public ShopBundleInfoDTO? bundle { get; set; }

    public string? colors { get; set; }
}