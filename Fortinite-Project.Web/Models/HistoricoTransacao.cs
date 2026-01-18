using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class HistoricoTransacao
{
    [Key]
    public int Id { get; set; }

    [InverseProperty("Usuario")]
    public int Usuario { get; set; }

    [InverseProperty("Cosmetico")]
    public int CosmeticoId { get; set; }
    public string TipoTransacao { get; set; } // "Compra" ou "Venda"
    public int Valor { get; set; }
    public DateTime DataTransacao { get; set; } = DateTime.Now;

}