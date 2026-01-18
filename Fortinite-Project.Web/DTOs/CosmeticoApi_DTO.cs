namespace Fortinite_Project.Web.DTOs;
public class CosmeticoApi_DTO
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Tipo { get; set; }
    public string Descricao { get; set; }
    public string Raridade { get; set; }
    public string UrlImagem { get; set; }
    public int Preco { get; set; } = 0;
    public bool isForSale { get; set; }
    public DateTime dataInclusao { get; set; }
    public bool isBundle { get; set; }
    public string bundleItemsJson { get; set; }
    public string coresJson { get; set; }
}
