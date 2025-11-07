using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TradusApp.Domain.Abstractions;
using TradusApp.Domain.Entities;
using TradusApp.Models.Chapters;

namespace TradusApp.Pages.Chapters;

public class CreateModel : PageModel
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CreateModel(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    [BindProperty]
    public ChapterCreateDto Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(Guid? libroId)
    {
        if (libroId is null || libroId == Guid.Empty)
        {
            TempData["ChapterBookError"] = "Debe crear o seleccionar un libro antes de crear capítulos.";
            return RedirectToPage("/Books/Manage");
        }
        var exists = await _uow.Repository<Libro>().Query().AnyAsync(l => l.Id == libroId);
        if (!exists)
        {
            TempData["ChapterBookError"] = "El libro indicado no existe. Cree primero el libro.";
            return RedirectToPage("/Books/Manage");
        }
        Input.LibroId = libroId.Value;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        if (string.IsNullOrWhiteSpace(Input.Contenido))
        {
            ModelState.AddModelError(nameof(Input.Contenido), "El capítulo no puede estar vacío.");
            return Page();
        }
        var book = await _uow.Repository<Libro>().Query().Include(l=>l.Capitulos).FirstOrDefaultAsync(l => l.Id == Input.LibroId);
        if (book == null)
        {
            TempData["ChapterBookError"] = "El libro no existe. Cree primero el libro y luego agregue el capítulo.";
            return RedirectToPage("/Books/Manage");
        }

        // Número automático si no se envia
        if (Input.Numero <=0)
        {
            Input.Numero = book.Capitulos.Any() ? book.Capitulos.Max(c=>c.Numero)+1 :1;
        }

        var entity = _mapper.Map<Capitulo>(Input);
        entity.LibroId = book.Id;
        entity.VersionActual =1;
        await _uow.Repository<Capitulo>().AddAsync(entity);
        await _uow.SaveChangesAsync();

        await _uow.Repository<ChapterVersion>().AddAsync(new ChapterVersion
        {
            CapituloId = entity.Id,
            Version =1,
            Contenido = entity.Contenido,
            Notas = "Versión inicial"
        });
        await _uow.SaveChangesAsync();

        return RedirectToPage("/Chapters/Details", new { id = entity.Id });
    }
}
