using Fortinite_Project.Web.Models;
using Microsoft.EntityFrameworkCore;


namespace Fortinite_Project.Web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Cosmetico> Cosmeticos { get; set; }

    public DbSet<Usuario> Usuarios { get; set; }

    public DbSet<HistoricoTransacao> HistoricoTransacoes { get; set; }

    public DbSet<ItemAdquirido> ItensAdquiridos { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }
}