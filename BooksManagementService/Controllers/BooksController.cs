using AutoMapper;
using BooksManagementService.DataTransferObjects;
using BooksManagementService.Models;
using Contracts;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BooksManagementService.Controllers;

/// <summary>
/// Controller for managing books.
/// Provides API endpoints for performing CRUD operations and rendering book-related views.
/// </summary>
[Route("api/books")]
[ApiController]
public class BooksController : Controller
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="BooksController"/> class.
    /// </summary>
    /// <param name="repository">Repository manager for handling database operations.</param>
    /// <param name="mapper">AutoMapper instance for transforming models.</param>
    public BooksController(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Returns the admin page for managing books.
    /// </summary>
    /// <returns>The admin page view.</returns>
    [HttpGet("admin")]
    public IActionResult BooksPageAdmin()
    {
        return View("~/Views/Books/AllBooksPage.cshtml");
    }

    /// <summary>
    /// Retrieves the total count of books based on query parameters.
    /// </summary>
    /// <param name="requestParameters">Request parameters including filters and pagination options.</param>
    /// <returns>The total count of books as an integer.</returns>
    [HttpGet("GetBooksCount")]
    public async Task<IActionResult> GetBooksCount([FromQuery] BookParameters requestParameters)
    {
        int booksCount = await _repository.Book.CountBooksAsync(requestParameters);
        return Ok(booksCount);
    }

    /// <summary>
    /// Retrieves a paginated list of books based on query parameters.
    /// </summary>
    /// <param name="requestParameters">Request parameters including filters and pagination options.</param>
    /// <returns>A paginated list of books with metadata about total pages and current page.</returns>
    [HttpGet("GetBooks")]
    public async Task<IActionResult> GetBooks([FromQuery] BookParameters requestParameters)
    {
        var books = await _repository.Book.GetAllBooksAsync(requestParameters, trackChanges: false);
        var booksDto = _mapper.Map<IEnumerable<BookDto>>(books);
        var totalBooks = await _repository.Book.CountBooksAsync(requestParameters); 
        var totalPages = (int)Math.Ceiling((double)totalBooks / requestParameters.PageSize);

        var response = new
        {
            books = booksDto,
            currentPage = requestParameters.PageNumber,
            totalPages
        };

        return Ok(response);
    }

    /// <summary>
    /// Retrieves a single book by its ID.
    /// </summary>
    /// <param name="id">The ID of the book.</param>
    /// <returns>The requested book's details or a 404 status if not found.</returns>
    [HttpGet("{id}", Name = "BookById")]
    public async Task<IActionResult> GetBook(int id)
    {
        var book = await _repository.Book.GetBookAsync(id, trackChanges: false);
        if (book == null)
        {
            return NotFound();
        }
        var bookDto = _mapper.Map<BookDto>(book);
        var genres = Enum.GetValues(typeof(BookGenre)).Cast<BookGenre>()
            .Select(g => new SelectListItem
            {
                Text = g.ToString(),
                Value = g.ToString(),
                Selected = g == bookDto.Genre 
            }).ToList();
        
        ViewBag.Genres = genres;  
        return Ok(bookDto);
    }

    /// <summary>
    /// Renders the book editing page for a specific book.
    /// </summary>
    /// <param name="id">The ID of the book to edit.</param>
    /// <returns>The edit book page view or a 404 status if not found.</returns>
    [HttpGet("edit/{id}", Name = "EditBook")]
    public async Task<IActionResult> EditBook(int id)
    {
        var book = await _repository.Book.GetBookAsync(id, trackChanges: false);
        if (book == null)
        {
            return NotFound();
        }

        var bookDto = _mapper.Map<BookDto>(book);

        var genres = Enum.GetValues(typeof(BookGenre)).Cast<BookGenre>()
            .Select(g => new SelectListItem
            {
                Text = g.ToString(),
                Value = g.ToString(),
                Selected = g == bookDto.Genre
            }).ToList();
        
        ViewBag.Genres = genres;  
        return View("~/Views/Books/EditBookPage.cshtml", bookDto);
    }

    /// <summary>
    /// Retrieves a book by its ISBN.
    /// </summary>
    /// <param name="ISBN">The ISBN of the book.</param>
    /// <returns>The book details or a 404 status if not found.</returns>
    [HttpGet("ByISBN/{ISBN}", Name = "BookByIsbn")]
    public async Task<IActionResult> GetBook(string ISBN)
    {
        var book = await _repository.Book.GetBookByISBNAsync(ISBN, trackChanges: false);
        if (book == null)
        {
            return NotFound();
        }
        var bookDto = _mapper.Map<BookDto>(book);
        return Ok(bookDto);
    }

    /// <summary>
    /// Renders the page for creating a new book.
    /// </summary>
    /// <returns>The create book page view.</returns>
    [HttpGet("AddBook")]
    public async Task<IActionResult> CreateBook()
    {
        var genres = Enum.GetValues(typeof(BookGenre)).Cast<BookGenre>()
            .Select(g => new SelectListItem
            {
                Text = g.ToString(),
                Value = g.ToString(),
                Selected = g == BookGenre.Adventures 
            }).ToList();
        
        ViewBag.Genres = genres; 
        return View("~/Views/Books/AddBookPage.cshtml");
    }

    /// <summary>
    /// Adds a new book to the database.
    /// </summary>
    /// <param name="book">The data for the book to create.</param>
    /// <returns>Status 200 if created successfully; otherwise, an error response.</returns>
    [HttpPost("add")]
    public async Task<IActionResult> CreateBook([FromBody]BookForCreationDto book)
    {
        if(book == null)
        {
            return BadRequest("BookForCreationDto object is null");
        }
        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
        }
        var bookEntity = _mapper.Map<Book>(book);
        _repository.Book.CreateBook(bookEntity);
        await _repository.SaveAsync();
        return Ok();
    }

    /// <summary>
    /// Deletes a book by its ID.
    /// </summary>
    /// <param name="id">The ID of the book to delete.</param>
    /// <returns>Status 204 if successfully deleted; otherwise, an error response.</returns>
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var book = await _repository.Book.GetBookAsync(id, trackChanges: false);
        if (book == null)
        {
            return NotFound();
        }
        _repository.Book.DeleteBook(book);
        await _repository.SaveAsync();
        return NoContent();
    }

    /// <summary>
    /// Updates a book's details.
    /// </summary>
    /// <param name="id">The ID of the book to update.</param>
    /// <param name="bookDto">The updated book details.</param>
    /// <returns>Status 204 if successfully updated; otherwise, an error response.</returns>
    [HttpPut("{id}", Name = "UpdateBook")]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] BookForUpdateDto bookDto)
    {
        var bookEntity = await _repository.Book.GetBookAsync(id, trackChanges: true);
        _mapper.Map(bookDto, bookEntity);
        await _repository.SaveAsync();
        return NoContent();
    }
}
