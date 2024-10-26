using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

public enum BookGenre
{
    Adventures = 1, 
    LoveStories = 2,
    Horrors = 3,
    FairyTales = 4
}

public class Book
{
    [Column("BookId")]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Book title is a required field.")]
    [MaxLength(13, ErrorMessage = "Maximum length for the title is 13 characters.")]
    [MinLength(10, ErrorMessage = "Minimum length for the title is 10 characters.")]
    public string ISBN { get; set; }

    [Required(ErrorMessage = "Book title is a required field.")]
    [MaxLength(30, ErrorMessage = "Maximum length for the title is 30 characters.")]
    public string BookTitle { get; set; }

    [Required(ErrorMessage = "Book genre is a required field.")]
    public BookGenre Genre { get; set; }

    [Required(ErrorMessage = "Description is a required field.")]
    public string Description { get; set; }
    [Required(ErrorMessage = "Author name is a required field.")]
    public string AuthorName { get; set; }
    [Required(ErrorMessage = "Author last name is a required field.")]
    public string AuthorLastName { get; set; }

    [Required(ErrorMessage = "Author ID is a required field.")]
    public int Amount { get; set; }
}
