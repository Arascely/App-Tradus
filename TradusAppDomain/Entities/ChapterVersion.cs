using System.ComponentModel.DataAnnotations;

namespace TradusApp.Domain.Entities;

public class ChapterVersion
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid CapituloId { get; set; }

    [Range(1, int.MaxValue)]
    public int Version { get; set; }

    [Required]
    public string Contenido { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Notas { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Nav
    public Capitulo? Capitulo { get; set; }
}
