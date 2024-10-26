using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BooksManagementService.Controllers;

[Route("api/books")]
[ApiController]
public class BooksController : Controller
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    public BooksController(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    [HttpGet("admin")]
    public IActionResult BooksPageAdmin()
    {
        return View("~/Views/Books/AllBooksPage.cshtml");
    }
    
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

    [HttpPut("{id}", Name = "UpdateBook")]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] BookForUpdateDto bookDto)
    {
        var bookEntity = await _repository.Book.GetBookAsync(id, trackChanges: true);
        _mapper.Map(bookDto, bookEntity);
        await _repository.SaveAsync();
        return NoContent();
    }
}
