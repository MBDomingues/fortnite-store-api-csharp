using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fortinite_Project.Web.Models;
public class ItemAdquirido
{
    [Key]
    public int Id { get; set; }

    [InverseProperty("ItemAdquirido")]
    public Usuario? Usuario { get; set; }

    [InverseProperty("ItemAdquirido")]
    public List<Cosmetico>? Cosmetico { get; set; }
    public DateTime DataCompra { get; set; } = DateTime.Now;
}