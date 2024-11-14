using LibraryManagementAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.Tests.Models;

public class BookTests
{
    [Fact]
    public void Book_ShouldHaveDefaultValues()
    {
        var book = new Book();

        Assert.Equal(string.Empty, book.BookTitle);
        Assert.Equal(string.Empty, book.ISBN);
        Assert.Equal(string.Empty, book.Genre);
        Assert.Equal(default, book.PublishedDate);
        Assert.Equal(0, book.AuthorId);
    }

    [Fact]
    public void GetTestBooks_ShouldReturnListOfBooks()
    {
        var count = 3;
        var books = Book.GetTestBooks();

        Assert.NotNull(books);
        Assert.Equal(count, books.Count);
        Assert.IsType<List<Book>>(books);
    }

    [Fact]
    public void Book_WithValidISBN13Digit_ShouldPassValidation()
    {
        var book = new Book
        {
            BookTitle = "Valid Book",
            ISBN = "978-3-16-148410-0", // Valid ISBN-13
            Genre = "Fiction",
            PublishedDate = DateTime.Now,
            AuthorId = 1
        };

        var validationResults = new List<ValidationResult>();
        // checks all the validation attributes applied to the class
        var isValid = Validator.TryValidateObject(book, new ValidationContext(book), validationResults, true);

        Assert.True(isValid);
        Assert.Empty(validationResults);
    }

    [Fact]
    public void Book_WithValidISBN10Digit_ShouldPassValidation()
    {
        var book = new Book
        {
            BookTitle = "Valid Book",
            ISBN = "0-306-40615-2", // Valid ISBN-10
            Genre = "Fiction",
            PublishedDate = DateTime.Now,
            AuthorId = 1
        };

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(book, new ValidationContext(book), validationResults, true);

        Assert.True(isValid);
        Assert.Empty(validationResults);
    }

    [Fact]
    public void Book_WithInvalidISBN_ShouldFailValidation()
    {
        var book = new Book
        {
            BookTitle = "Invalid Book",
            ISBN = "invalid-isbn", // Invalid ISBN
            Genre = "Fiction",
            PublishedDate = DateTime.Now,
            AuthorId = 1
        };

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(book, new ValidationContext(book), validationResults, true);

        Assert.False(isValid);
        Assert.Contains(validationResults, validation => validation.ErrorMessage == "Invalid ISBN format.");
    }
}
