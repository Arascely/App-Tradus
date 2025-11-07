using System.ComponentModel.DataAnnotations;

namespace TradusApp.Domain.Entities;

public class Libro
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(200)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Autor { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Idioma { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Descripcion { get; set; }

    [Range(0, 5)]
    public decimal CalificacionPromedio { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relaciones
    public ICollection<Capitulo> Capitulos { get; set; } = new List<Capitulo>();
    public ICollection<LibroEtiqueta> LibroEtiquetas { get; set; } = new List<LibroEtiqueta>();
}
