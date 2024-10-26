using BooksManagementService.Models;

namespace BooksManagementService.DataTransferObjects;

public class BookForUpdateDto
{
    public string ISBN { get; set; }
    public string BookTitle { get; set; }
    public BookGenre Genre { get; set; }
    public string Description { get; set; }
    public string AuthorName { get; set; }
    public string AuthorLastName { get; set; }
    public int Amount { get; set; }
}
