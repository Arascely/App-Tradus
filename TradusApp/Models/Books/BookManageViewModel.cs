using TradusApp.Models.Chapters;

namespace TradusApp.Models.Books;

public class BookManageViewModel
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public string Idioma { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int CapitulosCount { get; set; }
    public List<ChapterSummary> Capitulos { get; set; } = new();
    public BookManageChapterCreateDto NewChapter { get; set; } = new();
}

public class ChapterSummary
{
    public Guid Id { get; set; }
    public int Numero { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public int VersionActual { get; set; }
}

public class BookManageChapterCreateDto
{
    public Guid LibroId { get; set; }
    public int Numero { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Contenido { get; set; } = string.Empty;
}
