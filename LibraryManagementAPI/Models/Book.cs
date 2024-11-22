using LibraryManagementAPI.Interfaces;
using LibraryManagementAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.Models;

public class Book : IBook
{
    public int Id { get; set; }

    [Required(ErrorMessage = "title is required")]
    public string BookTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "isbn is required")]
    [ISBN]
    public string ISBN { get; set; } = string.Empty; // ISBN - International Standard Book Number

    [Required(ErrorMessage = "genre is required")]
    public string Genre { get; set; } = string.Empty;

    [Required(ErrorMessage = "publishedDate is required")]
    public DateTime PublishedDate { get; set; }

    [Required(ErrorMessage = "authorId is required")]
    public int AuthorId { get; set; }
}