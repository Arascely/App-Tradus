using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TradusApp.Domain.Abstractions;
using TradusApp.Domain.Entities;
using TradusApp.Models.Chapters;

namespace TradusApp.Pages.Chapters;

public class EditModel : PageModel
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public EditModel(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    [BindProperty]
    public ChapterEditDto Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var chapter = await _uow.Repository<Capitulo>().Query().AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        if (chapter == null) return new NotFoundResult();
        Input = _mapper.Map<ChapterEditDto>(chapter);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid id)
    {
        if (id != Input.Id) return BadRequest();
        if (!ModelState.IsValid) return Page();
        if (string.IsNullOrWhiteSpace(Input.Contenido))
        {
            ModelState.AddModelError(nameof(Input.Contenido), "El capítulo no puede estar vacío.");
            return Page();
        }

        var chapter = await _uow.Repository<Capitulo>().Query().FirstOrDefaultAsync(c => c.Id == id);
        if (chapter == null) return new NotFoundResult();

        bool contentChanged = !string.Equals(chapter.Contenido, Input.Contenido, StringComparison.Ordinal);
        bool titleChanged = !string.Equals(chapter.Titulo, Input.Titulo, StringComparison.Ordinal);
        if (contentChanged || titleChanged)
        {
            chapter.VersionActual += 1;
            await _uow.Repository<ChapterVersion>().AddAsync(new ChapterVersion
            {
                CapituloId = chapter.Id,
                Version = chapter.VersionActual,
                Contenido = Input.Contenido,
                Notas = titleChanged && !contentChanged ? "Cambio de título" : (contentChanged ? "Actualización de contenido" : "Actualización menor")
            });
        }

        chapter.Titulo = Input.Titulo;
        chapter.Contenido = Input.Contenido;
        chapter.UpdatedAt = DateTime.UtcNow;

        _uow.Repository<Capitulo>().Update(chapter);
        await _uow.SaveChangesAsync();
        return RedirectToPage("/Chapters/Details", new { id });
    }
}
