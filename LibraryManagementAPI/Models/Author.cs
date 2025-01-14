using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.Models;

public sealed class Author
{
    public int Id { get; set; }

    [Required(ErrorMessage = "authorName is required")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "authorName must be between 1 and 100 characters")]
    public string AuthorName { get; set; } = string.Empty;

    [Required(ErrorMessage = "biography is required")]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = "biography must be between 1 and 1000 characters")]
    public string Biography { get; set; } = string.Empty;

    public ICollection<Book> Books { get; set; } = new List<Book>();
}