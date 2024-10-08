using Microsoft.EntityFrameworkCore;
using RistrettoSistemas.Models;

namespace RistrettoSistemas.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Funcionario>()
                .HasIndex(f => f.CPF)
                .IsUnique();

            modelBuilder.Entity<Empresa>()
                .HasIndex(e => e.NomeEmpresarial)
                .IsUnique();
        }
    }
}
