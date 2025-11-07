using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TradusApp.Domain.Abstractions;
using TradusApp.Domain.Entities;
using TradusApp.Models.Chapters;
using X.PagedList;

namespace TradusApp.Pages.Chapters;

public class DetailsModel : PageModel
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public DetailsModel(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public ChapterDetailsPageViewModel ViewModel { get; set; } = new();

    [BindProperty]
    public CommentCreateDto NewComment { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(Guid id, string? search, int? page)
    {
        var chapter = await _uow.Repository<Capitulo>().Query()
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
        if (chapter == null) return new NotFoundResult();

        var chapterDto = _mapper.Map<ChapterDetailsDto>(chapter);
        var versions = await _uow.Repository<ChapterVersion>().Query()
            .Where(v => v.CapituloId == id)
            .OrderByDescending(v => v.Version)
            .ProjectTo<ChapterVersionItemDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        chapterDto.Versions = versions;

        int pageNumber = page.GetValueOrDefault(1);
        const int pageSize = 10;
        var commentsQuery = _uow.Repository<Comment>().Query().Where(c => c.CapituloId == id);
        if (!string.IsNullOrWhiteSpace(search))
            commentsQuery = commentsQuery.Where(c => c.Autor.Contains(search) || c.Contenido.Contains(search));
        commentsQuery = commentsQuery.OrderByDescending(c => c.CreatedAt);

        var commentsPaged = await commentsQuery
            .ProjectTo<CommentItemDto>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, pageSize);

        ViewModel = new ChapterDetailsPageViewModel
        {
            Chapter = chapterDto,
            Comments = commentsPaged,
            Search = search
        };

        NewComment = new CommentCreateDto { CapituloId = id };

        return Page();
    }

    public async Task<IActionResult> OnPostAddCommentAsync()
    {
        if (!ModelState.IsValid || string.IsNullOrWhiteSpace(NewComment.Contenido))
        {
            if (string.IsNullOrWhiteSpace(NewComment.Contenido))
                ModelState.AddModelError("NewComment.Contenido", "El comentario no puede estar vacío.");
            // recargar datos mínimos para mostrar nuevamente la página
            return await OnGetAsync(NewComment.CapituloId, null, 1);
        }

        var chapterExists = await _uow.Repository<Capitulo>().Query().AnyAsync(c => c.Id == NewComment.CapituloId);
        if (!chapterExists) return new NotFoundResult();

        var comment = new Comment { CapituloId = NewComment.CapituloId, Autor = NewComment.Autor, Contenido = NewComment.Contenido };
        await _uow.Repository<Comment>().AddAsync(comment);
        await _uow.SaveChangesAsync();
        return RedirectToPage(new { id = NewComment.CapituloId });
    }
}
