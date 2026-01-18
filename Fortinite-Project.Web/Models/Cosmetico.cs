using System.ComponentModel.DataAnnotations;

public class Cosmetico
{   
    [Key]
    public int Id { get; set; }

    [MaxLength(100)]
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public string Tipo { get; set; }
    public string Raridade { get; set; }
    public string UrlImagem { get; set; }
    public int Preco { get; set; } = 0;
    public bool isForSale { get; set; } = false;
    public DateTime dataInclusao { get; set; } = DateTime.Now;
    public bool isBundle { get; set; } = false;
    public string bundleItemsJson { get; set; } = "[]";
    public string coresJson { get; set; } = "[]";
}