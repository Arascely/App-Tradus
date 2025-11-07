using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TradusApp.Domain.Abstractions;
using TradusApp.Domain.Entities;
using TradusApp.Models.Books;
using X.PagedList;

namespace TradusApp.Pages.Books;

public class IndexModel : PageModel
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public IndexModel(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    [BindProperty(SupportsGet = true)] public string? Search { get; set; }
    [BindProperty(SupportsGet = true)] public string? Author { get; set; }
    [BindProperty(SupportsGet = true)] public string? Language { get; set; }
    [BindProperty(SupportsGet = true)] public int? Page { get; set; }

    public IPagedList<BookListItemDto> Books { get; private set; }
        = new PagedList<BookListItemDto>(Enumerable.Empty<BookListItemDto>(), 1, 10);

    public async Task OnGetAsync()
    {
        const int pageSize = 10;
        var page = Page ?? 1;

        var query = _uow.Repository<Libro>().Query().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(Search))
            query = query.Where(x => x.Titulo.Contains(Search));
        if (!string.IsNullOrWhiteSpace(Author))
            query = query.Where(x => x.Autor.Contains(Author));
        if (!string.IsNullOrWhiteSpace(Language))
            query = query.Where(x => x.Idioma.Contains(Language));

        query = query.OrderByDescending(x => x.CreatedAt);

        var projected = query.ProjectTo<BookListItemDto>(_mapper.ConfigurationProvider);
        Books = await projected.ToPagedListAsync(page, pageSize);
    }
}
