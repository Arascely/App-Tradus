using X.PagedList;

namespace TradusApp.Models.Chapters;

public class ChapterDetailsPageViewModel
{
    public ChapterDetailsDto Chapter { get; set; } = new();
    public IPagedList<CommentItemDto> Comments { get; set; } = new PagedList<CommentItemDto>(new List<CommentItemDto>(), 1, 10);
    public string? Search { get; set; }
}
