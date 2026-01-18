using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ItemAdquirido
{
    [Key]
    public int Id { get; set; }

    [InverseProperty("Usuario")]
    public int Usuario { get; set; }
    
    [InverseProperty("Cosmetico")]
    public int Cosmetico { get; set; }
    public DateTime DataCompra { get; set; } = DateTime.Now;
}