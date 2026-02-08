using System.Text.Json.Serialization;

namespace Fortinite_Project.Web.DTOs;

public class CosmeticoApi_DTO
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string? Nome { get; set; }

    [JsonPropertyName("description")]
    public string? Descricao { get; set; }

    // O JSON mostra que 'type' é um objeto, não string
    [JsonPropertyName("type")]
    public FortniteTypeInfo? TypeInfo { get; set; }

    // O JSON mostra que 'images' é um objeto
    [JsonPropertyName("images")]
    public FortniteImages? Images { get; set; }

    [JsonPropertyName("series")]
    public FortniteSeriesInfo? Series { get; set; }

    [JsonPropertyName("rarity")]
    public FortniteRarityInfo? Rarity { get; set; }

    // Campos simples
    [JsonPropertyName("price")] 
    public int Preco { get; set; } = 0;

    [JsonPropertyName("added")]
    public DateTime dataInclusao { get; set; } = DateTime.Now;
}
public class FortniteTypeInfo
{
    [JsonPropertyName("displayValue")]
    public string? DisplayValue { get; set; }
    
    [JsonPropertyName("value")]
    public string? Value { get; set; }
}
public class FortniteImages
{
    [JsonPropertyName("small")]
    public string? Small { get; set; }
    
    [JsonPropertyName("large")]
    public string? Large { get; set; }
    
    [JsonPropertyName("icon")]
    public string? Icon { get; set; }
}
public class FortniteSeriesInfo
{
    [JsonPropertyName("value")]
    public string? Value { get; set; }
}
public class FortniteRarityInfo
{
    [JsonPropertyName("displayValue")]
    public string? DisplayValue { get; set; }
}