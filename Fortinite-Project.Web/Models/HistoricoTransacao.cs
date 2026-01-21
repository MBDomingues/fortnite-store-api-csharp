using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fortinite_Project.Web.Models;
public class HistoricoTransacao
{
    [Key]
    public int Id { get; set; }

    [InverseProperty("HistoricoTransacao")]
    public Usuario? Usuario { get; set; }

    [InverseProperty("HistoricoTransacao")]
    public Cosmetico? Cosmetico { get; set; }
    public string? TipoTransacao { get; set; } // "Compra" ou "Venda"
    public int Valor { get; set; }
    public DateTime DataTransacao { get; set; } = DateTime.Now;

}