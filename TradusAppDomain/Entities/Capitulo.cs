using System.ComponentModel.DataAnnotations;

namespace TradusApp.Domain.Entities;

public class Capitulo
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid LibroId { get; set; }

    [Required]
    [StringLength(200)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    public int Numero { get; set; }

    [Required]
    public string Contenido { get; set; } = string.Empty;

    public int VersionActual { get; set; } = 1;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navegación
    public Libro? Libro { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<ChapterVersion> Versions { get; set; } = new List<ChapterVersion>();
    public ICollection<Report> Reports { get; set; } = new List<Report>();
}
