using Microsoft.EntityFrameworkCore;
using Fortinite_Project.Web.Models;

namespace Fortinite_Project.Web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Cosmetico> Cosmeticos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }
}