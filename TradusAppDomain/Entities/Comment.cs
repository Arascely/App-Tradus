using System.ComponentModel.DataAnnotations;

namespace TradusApp.Domain.Entities;

public class Comment
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid CapituloId { get; set; }

    [Required]
    [StringLength(100)]
    public string Autor { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Contenido { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navegación
    public Capitulo? Capitulo { get; set; }
}
