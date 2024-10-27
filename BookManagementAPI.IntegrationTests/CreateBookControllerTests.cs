using System.Net;
using System.Net.Http.Json;
using BooksManagementService.DataTransferObjects;
using BooksManagementService.Models;
using Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookManagementAPI.IntegrationTests;

[TestFixture]
public class CreateBookControllerTests
{
    private HttpClient? _client;

    [SetUp]
    public void SetUp()
    {
        var appFactory = new WebApplicationFactory<Program>(); 
        _client = appFactory.CreateClient();
    }
    
    [TearDown]
    public void TearDown()
    {
        _client?.Dispose();
    }

    [Test]
    public async Task GetBook_ExistingBookId_ShouldReturnBookDto()
    {
        var bookId = 8; 

        var response = await _client.GetAsync($"/api/books/{bookId}");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var bookDto = await response.Content.ReadFromJsonAsync<BookDto>();

        Assert.IsNotNull(bookDto);
        Assert.AreEqual(bookId, bookDto.Id);
    }

    [Test]
    public async Task GetBook_NonExistingBookId_ShouldReturnNotFound()
    {
        var nonExistingBookId = 999;

        var response = await _client.GetAsync($"/api/books/{nonExistingBookId}");

        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Test]
    public async Task GetBooksCount_SendRequest_ShouldReturnActualBooksCounts()
    {
        WebApplicationFactory<Program> webHost = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var dbContextDescriptor = services.SingleOrDefault(d =>
                        d.ServiceType == typeof(DbContextOptions<ApplicationContext>));
                    services.Remove(dbContextDescriptor);
                    services.AddDbContext<ApplicationContext>(options =>
                    {
                        options.UseInMemoryDatabase("books_db");
                    });
                });
            });

        ApplicationContext dbContext = webHost.Services.CreateScope().ServiceProvider
            .GetService<ApplicationContext>()!;
        List<Book> books = new() 
        { 
            new Book()
            {
                BookTitle = "New Book",
                ISBN = "987654321",
                Genre = BookGenre.FairyTales,
                Description = "Horror tale",
                Amount = 3,
                AuthorName = "Eugen",
                AuthorLastName = "Savenok"
            }, 
            new Book()
            {
                BookTitle = "New Book",
                ISBN = "987654321",
                Genre = BookGenre.FairyTales,
                Description = "Horror tale",
                Amount = 3,
                AuthorName = "Eugen",
                AuthorLastName = "Savenok"
            }, 
            new Book()
            {
                BookTitle = "New Book",
                ISBN = "987654321",
                Genre = BookGenre.FairyTales,
                Description = "Horror tale",
                Amount = 3,
                AuthorName = "Eugen",
                AuthorLastName = "Savenok"
            }
        };
        await dbContext.Books.AddRangeAsync(books);
        await dbContext.SaveChangesAsync();
        HttpClient httpClient = webHost.CreateClient();
        HttpResponseMessage responseMessage = await httpClient.GetAsync("api/books/GetBooksCount");
        Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);
        int booksCount = int.Parse(await responseMessage.Content.ReadAsStringAsync());
        Assert.AreEqual(books.Count, booksCount);
    }
}
