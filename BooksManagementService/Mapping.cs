using AutoMapper;
using BooksManagementService.DataTransferObjects;
using BooksManagementService.Models;

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

