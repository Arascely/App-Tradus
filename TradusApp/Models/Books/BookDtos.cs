using System.ComponentModel.DataAnnotations;

namespace TradusApp.Models.Books;

public class BookListItemDto
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public string Idioma { get; set; } = string.Empty;
    public decimal CalificacionPromedio { get; set; }
    public int Capitulos { get; set; }
}

public class BookCreateDto
{
    [Required, StringLength(200)]
    public string Titulo { get; set; } = string.Empty;
    [Required, StringLength(100)]
    public string Autor { get; set; } = string.Empty;
    [Required, StringLength(50)]
    public string Idioma { get; set; } = string.Empty;
    [StringLength(2000)]
    public string? Descripcion { get; set; }
}

public class BookEditDto : BookCreateDto
{
    [Required]
    public Guid Id { get; set; }
}

public class CommentDto
{
    public Guid Id { get; set; }
    public string Autor { get; set; } = string.Empty;
    public string Contenido { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class ChapterDto
{
    public Guid Id { get; set; }
    public int Numero { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public int CommentsCount { get; set; }
    public List<CommentDto> Comments { get; set; } = new();
}

public class BookDetailsDto
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public string Idioma { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal CalificacionPromedio { get; set; }
    public List<ChapterDto> Capitulos { get; set; } = new();
}
