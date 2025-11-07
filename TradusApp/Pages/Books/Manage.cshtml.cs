using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TradusApp.Domain.Abstractions;
using TradusApp.Domain.Entities;
using TradusApp.Models.Books;

namespace TradusApp.Pages.Books;

public class ManageModel : PageModel
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public ManageModel(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    [BindProperty(SupportsGet = true)] public Guid? Id { get; set; }

    [BindProperty] public BookManageViewModel? Book { get; set; }

    public List<BookListItemDto> BookList { get; set; } = new();

    public async Task OnGetAsync()
    {
        // Lista compacta de libros
        BookList = await _uow.Repository<Libro>().Query()
            .AsNoTracking()
            .OrderByDescending(l => l.CreatedAt)
            .Select(l => new BookListItemDto
            {
                Id = l.Id,
                Titulo = l.Titulo,
                Autor = l.Autor,
                Idioma = l.Idioma,
                CalificacionPromedio = l.CalificacionPromedio,
                Capitulos = l.Capitulos.Count
            }).ToListAsync();

        if (Id.HasValue && Id != Guid.Empty)
        {
            var libro = await _uow.Repository<Libro>().Query()
                .Include(l => l.Capitulos)
                .FirstOrDefaultAsync(l => l.Id == Id.Value);
            if (libro != null)
            {
                Book = new BookManageViewModel
                {
                    Id = libro.Id,
                    Titulo = libro.Titulo,
                    Autor = libro.Autor,
                    Idioma = libro.Idioma,
                    Descripcion = libro.Descripcion,
                    CapitulosCount = libro.Capitulos.Count,
                    Capitulos = libro.Capitulos
                        .OrderBy(c => c.Numero)
                        .Select(c => new ChapterSummary
                        {
                            Id = c.Id,
                            Numero = c.Numero,
                            Titulo = c.Titulo,
                            VersionActual = c.VersionActual
                        }).ToList(),
                    NewChapter = new BookManageChapterCreateDto
                    {
                        LibroId = libro.Id,
                        Numero = libro.Capitulos.Count + 1
                    }
                };
            }
        }
    }

    public async Task<IActionResult> OnPostUpdateBookAsync()
    {
        if (Book == null) return RedirectToPage();
        var libro = await _uow.Repository<Libro>().Query().FirstOrDefaultAsync(l => l.Id == Book.Id);
        if (libro == null) return RedirectToPage();

        libro.Titulo = Book.Titulo.Trim();
        libro.Autor = Book.Autor.Trim();
        libro.Idioma = Book.Idioma.Trim();
        libro.Descripcion = Book.Descripcion?.Trim();
        libro.UpdatedAt = DateTime.UtcNow;
        _uow.Repository<Libro>().Update(libro);
        await _uow.SaveChangesAsync();
        return RedirectToPage(new { id = Book.Id });
    }

    public async Task<IActionResult> OnPostCreateChapterAsync()
    {
        if (Book?.NewChapter == null) return RedirectToPage(new { id = Book?.Id });
        if (string.IsNullOrWhiteSpace(Book.NewChapter.Titulo) || string.IsNullOrWhiteSpace(Book.NewChapter.Contenido))
        {
            ModelState.AddModelError("Book.NewChapter.Titulo", "Título requerido");
            ModelState.AddModelError("Book.NewChapter.Contenido", "Contenido requerido");
            return await ReloadPage();
        }

        var libro = await _uow.Repository<Libro>().Query().FirstOrDefaultAsync(l => l.Id == Book.NewChapter.LibroId);
        if (libro == null) return RedirectToPage();

        var cap = new Capitulo
        {
            LibroId = libro.Id,
            Numero = Book.NewChapter.Numero,
            Titulo = Book.NewChapter.Titulo.Trim(),
            Contenido = Book.NewChapter.Contenido.Trim(),
            VersionActual = 1,
            CreatedAt = DateTime.UtcNow
        };

        await _uow.Repository<Capitulo>().AddAsync(cap);
        await _uow.SaveChangesAsync();
        await _uow.Repository<ChapterVersion>().AddAsync(new ChapterVersion
        {
            CapituloId = cap.Id,
            Version = 1,
            Contenido = cap.Contenido,
            Notas = "Versión inicial"
        });
        await _uow.SaveChangesAsync();

        return RedirectToPage(new { id = libro.Id });
    }

    public async Task<IActionResult> OnPostDeleteBookAsync()
    {
        if (Book == null) return RedirectToPage();
        var libro = await _uow.Repository<Libro>().Query().Include(l => l.Capitulos).FirstOrDefaultAsync(l => l.Id == Book.Id);
        if (libro == null) return RedirectToPage();
        _uow.Repository<Libro>().Remove(libro);
        await _uow.SaveChangesAsync();
        return RedirectToPage();
    }

    private async Task<PageResult> ReloadPage()
    {
        await OnGetAsync();
        return Page();
    }
}
