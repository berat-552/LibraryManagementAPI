using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Interfaces;

public interface IAuthor
{
    int Id { get; set; }
    string AuthorName { get; set; }
    string Biography { get; set; }
    ICollection<Book> Books { get; set; }
}
