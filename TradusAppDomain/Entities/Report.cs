using System.ComponentModel.DataAnnotations;

namespace TradusApp.Domain.Entities;

public class Report
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid CapituloId { get; set; }

    [Required]
    [StringLength(100)]
    public string Autor { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Motivo { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Nav
    public Capitulo? Capitulo { get; set; }
}
