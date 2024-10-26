using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace BooksManagementService;

public class Mapping : Profile
{
    public Mapping()
    {
        CreateMap<Book, BookDto>();
        CreateMap<BookForCreationDto, Book>();
        CreateMap<BookForUpdateDto, Book>();
    }
}

