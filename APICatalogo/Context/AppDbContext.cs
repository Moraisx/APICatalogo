using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Produto>? Produtos { get; set; }
        public DbSet<Categoria>? Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Categoria>()
                .ToTable("Categorias");

            modelBuilder.Entity<Categoria>()
                .HasKey(p => p.CategoriaId);

            modelBuilder.Entity<Categoria>()
                .HasIndex(p => new {p.CategoriaId});

            modelBuilder.Entity<Categoria>()
                .Property(p => p.CategoriaId)
                .HasColumnName("CategoriaId")
                .HasColumnType("int");
               
            modelBuilder.Entity<Categoria>()
                .Property(p => p.Nome)
                .HasColumnName("Nome")
                .HasColumnType("varchar")
                .HasMaxLength(80)
                .IsRequired();

            modelBuilder.Entity<Categoria>()
                .Property(p => p.ImagemUrl)
                .HasColumnName("ImagemUrl")
                .HasColumnType("varchar")
                .HasMaxLength(300)
                .IsRequired();

            modelBuilder.Entity<Categoria>()
                .HasMany(p => p.Produtos)
                .WithOne(g => g.Categoria);
        }
    } 
}
