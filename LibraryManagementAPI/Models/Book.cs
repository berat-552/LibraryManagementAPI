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

    public static List<Book> GetTestBooks()
    {
        return
            [
                new Book
                {
                    Id = 1,
                    BookTitle = "Pride and Prejudice",
                    ISBN = "978-3-16-148410-0",
                    Genre = "Romance",
                    AuthorId = 1,
                    PublishedDate = new DateTime(1813, 1, 28),
                },
                new Book
                {
                    Id = 2,
                    BookTitle = "Adventures of Huckleberry Finn",
                    ISBN = "978-0-14-243717-9",
                    Genre = "Adventure",
                    AuthorId = 2,
                    PublishedDate = new DateTime(1884, 12, 10),
                },
                new Book
                {
                    Id = 3,
                    BookTitle = "Great Expectations",
                    ISBN = "978-0-14-143956-3",
                    Genre = "Fiction",
                    AuthorId = 3,
                    PublishedDate = new DateTime(1861, 8, 1),
                }
            ];
    }
}
