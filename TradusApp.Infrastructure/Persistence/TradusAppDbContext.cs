using Microsoft.EntityFrameworkCore;
using TradusApp.Domain.Entities;

namespace TradusApp.Infrastructure.Persistence;

public class TradusAppDbContext : DbContext
{
    public TradusAppDbContext(DbContextOptions<TradusAppDbContext> options) : base(options)
    {
    }

    public DbSet<Libro> Libros => Set<Libro>();
    public DbSet<Capitulo> Capitulos => Set<Capitulo>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Etiqueta> Etiquetas => Set<Etiqueta>();
    public DbSet<LibroEtiqueta> LibroEtiquetas => Set<LibroEtiqueta>();
    public DbSet<ChapterVersion> ChapterVersions => Set<ChapterVersion>();
    public DbSet<Report> Reports => Set<Report>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Libro>(entity =>
        {
            entity.ToTable("Libros");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Titulo).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Autor).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Idioma).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Descripcion).HasMaxLength(2000);
            entity.Property(e => e.CalificacionPromedio)
                  .HasPrecision(2, 1) // 0.0 - 5.0
                  .HasDefaultValue(0m);
            entity.HasMany(e => e.Capitulos)
                  .WithOne(c => c.Libro)
                  .HasForeignKey(c => c.LibroId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => e.Titulo);
            entity.HasIndex(e => e.Autor);
            entity.HasIndex(e => e.Idioma);
        });

        modelBuilder.Entity<Capitulo>(entity =>
        {
            entity.ToTable("Capitulos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Titulo).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Numero).IsRequired();
            entity.Property(e => e.Contenido).IsRequired();
            entity.Property(e => e.VersionActual).IsRequired();
            entity.HasIndex(e => new { e.LibroId, e.Numero }).IsUnique();
            entity.HasMany(e => e.Comments)
                  .WithOne(c => c.Capitulo)
                  .HasForeignKey(c => c.CapituloId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.Versions)
                  .WithOne(v => v.Capitulo)
                  .HasForeignKey(v => v.CapituloId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.Reports)
                  .WithOne(r => r.Capitulo)
                  .HasForeignKey(r => r.CapituloId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ChapterVersion>(entity =>
        {
            entity.ToTable("ChapterVersions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Version).IsRequired();
            entity.Property(e => e.Contenido).IsRequired();
            entity.Property(e => e.Notas).HasMaxLength(500);
            entity.HasIndex(e => new { e.CapituloId, e.Version }).IsUnique();
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comments");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Autor).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Contenido).IsRequired().HasMaxLength(1000);
        });

        modelBuilder.Entity<Etiqueta>(entity =>
        {
            entity.ToTable("Etiquetas");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.Nombre).IsUnique();
        });

        modelBuilder.Entity<LibroEtiqueta>(entity =>
        {
            entity.ToTable("LibroEtiquetas");
            entity.HasKey(e => new { e.LibroId, e.EtiquetaId });
            entity.HasOne(e => e.Libro)
                  .WithMany(l => l.LibroEtiquetas)
                  .HasForeignKey(e => e.LibroId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Etiqueta)
                  .WithMany(t => t.LibroEtiquetas)
                  .HasForeignKey(e => e.EtiquetaId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.ToTable("Reports");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Autor).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Motivo).IsRequired().HasMaxLength(1000);
        });
    }
}
