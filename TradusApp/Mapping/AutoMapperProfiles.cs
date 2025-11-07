using AutoMapper;
using TradusApp.Domain.Entities;
using TradusApp.Models.Books;
using TradusApp.Models.Chapters;

namespace TradusApp.Mapping;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        // Libros
        CreateMap<Libro, BookListItemDto>()
            .ForMember(d => d.Capitulos, opt => opt.MapFrom(s => s.Capitulos.Count));

        CreateMap<BookCreateDto, Libro>();
        CreateMap<BookEditDto, Libro>();
        CreateMap<Libro, BookEditDto>();

        CreateMap<Comment, CommentDto>();
        CreateMap<Capitulo, ChapterDto>()
            .ForMember(d => d.CommentsCount, opt => opt.MapFrom(s => s.Comments.Count))
            .ForMember(d => d.Comments, opt => opt.MapFrom(s => s.Comments));

        CreateMap<Libro, BookDetailsDto>();

        // Chapters
        CreateMap<ChapterCreateDto, Capitulo>();
        CreateMap<ChapterEditDto, Capitulo>();
        CreateMap<Capitulo, ChapterEditDto>();
        CreateMap<Capitulo, ChapterDetailsDto>();
        CreateMap<ChapterVersion, ChapterVersionItemDto>()
            .ForMember(d => d.Fecha, opt => opt.MapFrom(s => s.CreatedAt));
        CreateMap<Comment, CommentItemDto>()
            .ForMember(d => d.Fecha, opt => opt.MapFrom(s => s.CreatedAt));
    }
}
