using System.ComponentModel.DataAnnotations;

namespace TradusApp.Models.Chapters;

public class ChapterCreateDto
{
    [Required]
    public Guid LibroId { get; set; }

    [Required, Range(1, int.MaxValue)]
    public int Numero { get; set; }

    [Required, StringLength(200)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    public string Contenido { get; set; } = string.Empty;
}

public class ChapterEditDto
{
    [Required]
    public Guid Id { get; set; }

    [Required, StringLength(200)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    public string Contenido { get; set; } = string.Empty;

    public int VersionActual { get; set; }
}

public class ChapterVersionCreateDto
{
    [Required]
    public Guid CapituloId { get; set; }

    [Required]
    public string Contenido { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Notas { get; set; }
}

public class ChapterDetailsDto
{
    public Guid Id { get; set; }
    public Guid LibroId { get; set; }
    public int Numero { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Contenido { get; set; } = string.Empty;
    public int VersionActual { get; set; }
    public List<ChapterVersionItemDto> Versions { get; set; } = new();
    public List<CommentItemDto> Comments { get; set; } = new();
}

public class ChapterVersionItemDto
{
    public Guid Id { get; set; }
    public int Version { get; set; }
    public string Notas { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    // Nombre anterior expuesto para las vistas
    public DateTime Fecha { get; set; }
}

public class CommentItemDto
{
    public Guid Id { get; set; }
    public string Autor { get; set; } = string.Empty;
    public string Contenido { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    // Nombre anterior expuesto para las vistas
    public DateTime Fecha { get; set; }
}

public class ReportCreateDto
{
    [Required]
    public Guid CapituloId { get; set; }

    [Required, StringLength(100)]
    public string Autor { get; set; } = string.Empty;

    [Required, StringLength(1000)]
    public string Motivo { get; set; } = string.Empty;
}
