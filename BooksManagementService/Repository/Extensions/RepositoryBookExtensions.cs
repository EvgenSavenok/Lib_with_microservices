using BooksManagementService.Models;

namespace Repository.Extensions;

public static class RepositoryBookExtensions
{ 
    public static IQueryable<Book> Search(this IQueryable<Book> books, string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm) || searchTerm == "null")
            return books;
        var lowerCaseTerm = searchTerm.Trim().ToLower();
        return books.Where(b => b.BookTitle.ToLower().Contains(lowerCaseTerm));
    }
}
