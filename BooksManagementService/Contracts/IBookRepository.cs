using BooksManagementService.Models;
using Entities.RequestFeatures;

namespace Contracts;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllBooksAsync(BookParameters bookParameters, bool trackChanges);
    Task<Book> GetBookAsync(int bookId, bool trackChanges);
    Task<Book> GetBookByISBNAsync(string bookIsbn, bool trackChanges);
    Task<int> CountBooksAsync(BookParameters requestParameters);
    void CreateBook(Book book);
    void DeleteBook(Book book);
}
