using System.ComponentModel.DataAnnotations;

namespace TradusApp.Domain.Entities;

public class Etiqueta
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(50)]
    public string Nombre { get; set; } = string.Empty;

    // Relaciones
    public ICollection<LibroEtiqueta> LibroEtiquetas { get; set; } = new List<LibroEtiqueta>();
}
