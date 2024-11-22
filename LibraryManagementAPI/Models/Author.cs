using LibraryManagementAPI.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.Models;

public class Author : IAuthor
{
    public int Id { get; set; }

    [Required(ErrorMessage = "authorName is required")]
    public string AuthorName { get; set; } = string.Empty;

    [Required(ErrorMessage = "biography is required")]
    public string Biography { get; set; } = string.Empty;

    public ICollection<Book> Books { get; set; } = new List<Book>();
}