using System.ComponentModel.DataAnnotations;

namespace TradusApp.Models.Chapters;

public class CommentCreateDto
{
    [Required]
    public Guid CapituloId { get; set; }

    [Required, StringLength(100)]
    public string Autor { get; set; } = string.Empty;

    [Required, StringLength(1000)]
    public string Contenido { get; set; } = string.Empty;
}

public class CommentEditDto
{
    [Required]
    public Guid Id { get; set; }

    [Required, StringLength(100)]
    public string Autor { get; set; } = string.Empty;

    [Required, StringLength(1000)]
    public string Contenido { get; set; } = string.Empty;
}
