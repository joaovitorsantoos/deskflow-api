using DeskFlow.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeskFlow.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Categoria> Categorias { get; set; }

    public DbSet<Chamado> Chamados { get; set; }

    public DbSet<Interacao> Interacoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Categoria>()
            .HasMany(c => c.Chamados)
            .WithOne(c => c.Categoria)
            .HasForeignKey(c => c.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Chamado>()
            .HasMany(c => c.Interacoes)
            .WithOne(i => i.Chamado)
            .HasForeignKey(i => i.ChamadoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}