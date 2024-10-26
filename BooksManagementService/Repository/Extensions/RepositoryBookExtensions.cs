using Entities.Models;

namespace Repository.Extensions;

public static class RepositoryBookExtensions
{ 
    public static IQueryable<Book> Search(this IQueryable<Book> books, string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm.Equals(null))
            return books;
        var lowerCaseTerm = searchTerm.Trim().ToLower();
        return books.Where(b => b.BookTitle.ToLower().Contains(lowerCaseTerm));
    }
}
