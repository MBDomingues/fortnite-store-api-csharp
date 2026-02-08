using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fortinite_Project.Web.Models;
public class Usuario
{
    [Key]
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public int creditos { get; set; } = 10000;
    public DateTime DataCadastro { get; set; } = DateTime.Now;

    [InverseProperty("Usuario")]
    public List<HistoricoTransacao> HistoricoTransacao { get; set; } = new();
 
    [InverseProperty("Usuario")]
    public List<ItemAdquirido>? ItemAdquirido { get; set; }
}