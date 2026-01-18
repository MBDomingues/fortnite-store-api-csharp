using System.ComponentModel.DataAnnotations;

public class Usuario
{
    [Key]
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public int creditos { get; set; } = 10000;
    public DateTime DataCadastro { get; set; } = DateTime.Now;
}