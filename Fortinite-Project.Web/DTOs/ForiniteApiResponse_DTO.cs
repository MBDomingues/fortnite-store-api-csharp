using System.Text.Json.Serialization;

namespace Fortinite_Project.Web.DTOs;

public class FortniteApiResponse_DTO
{
    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("data")]
    public List<CosmeticoApi_DTO> Data { get; set; } = new();
}