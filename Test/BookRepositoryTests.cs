using Microsoft.EntityFrameworkCore;
using BooksManagementService.Models;
using Entities;
using FluentAssertions;
using Repository;

namespace Test;

public class BookRepositoryTests
{
    private DbContextOptions<ApplicationContext> _dbContextOptions;

    public BookRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "TestLibraryDb")
            .Options;
    }
    
    [Fact]
    public async Task GetBookAsync_ShouldReturnBook_WhenBookExists()
    {
        var book = new Book
        {
            Id = 1,
            BookTitle = "Test Book",
            ISBN = "123456789",
            Genre = BookGenre.Horrors,
            Description = "Horror tale",
            Amount = 3,
            AuthorName = "Eugen",
            AuthorLastName = "Savenok"
        };

        await using (var context = new ApplicationContext(_dbContextOptions))
        {
            context.Books.Add(book);
            await context.SaveChangesAsync();
        }

        await using (var context = new ApplicationContext(_dbContextOptions))
        {
            var repository = new BookRepository(context);
            var result = await repository.GetBookAsync(1, trackChanges: false);

            result.Should().NotBeNull();
            result!.BookTitle.Should().Be("Test Book");
            result.ISBN.Should().Be("123456789");
            result.Genre.Should().Be(BookGenre.Horrors);
            result.Description.Should().Be("Horror tale");
            result.Amount.Should().Be(3);
            result.AuthorName.Should().Be("Eugen");
            result.AuthorLastName.Should().Be("Savenok");
        }
    }
    
    [Fact]
    public async Task GetBookAsync_ShouldReturnNull_WhenBookDoesNotExist()
    {
        await using (var context = new ApplicationContext(_dbContextOptions))
        {
            var repository = new BookRepository(context);
            var result = await repository.GetBookAsync(100, trackChanges: false);

            result.Should().BeNull(); 
        }
    }

    [Fact]
    public async Task CreateBook_ShouldAddBookToDatabase()
    {
        var newBook = new Book
        {
            BookTitle = "New Book",
            ISBN = "987654321",
            Genre = BookGenre.FairyTales,
            Description = "Horror tale",
            Amount = 3,
            AuthorName = "Eugen",
            AuthorLastName = "Savenok"
        };

        await using (var context = new ApplicationContext(_dbContextOptions))
        {
            var repository = new BookRepository(context);
            repository.CreateBook(newBook);
            await context.SaveChangesAsync(); 
        }

        await using (var context = new ApplicationContext(_dbContextOptions))
        {
            var addedBook = await context.Books.FirstOrDefaultAsync(b => b.ISBN == "987654321");
            addedBook.Should().NotBeNull();
            addedBook!.BookTitle.Should().Be("New Book");
            addedBook.Genre.Should().Be(BookGenre.FairyTales);
            addedBook.Description.Should().Be("Horror tale");
            addedBook.Amount.Should().Be(3);
            addedBook.AuthorName.Should().Be("Eugen");
            addedBook.AuthorLastName.Should().Be("Savenok");
        }
    }
}
