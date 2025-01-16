using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace Tests.Models;

public class BookTests
{
    private static Book CreateBook(string isbn) => new()
    {
        BookTitle = "Book",
        Isbn = isbn,
        Genre = "Fiction",
        PublishedDate = DateTime.Now,
        AuthorId = 1
    };

    [Fact]
    public void Book_ShouldHaveDefaultValues()
    {
        var book = new Book();

        Assert.Equal(string.Empty, book.BookTitle);
        Assert.Equal(string.Empty, book.Isbn);
        Assert.Equal(string.Empty, book.Genre);
        Assert.Equal(default, book.PublishedDate);
        Assert.Equal(0, book.AuthorId);
    }

    [Fact]
    public void SeedBooks_ShouldReturnListOfBooks()
    {
        var count = 9;
        var books = SeedData.SeedBooks();

        Assert.NotNull(books);
        Assert.Equal(count, books.Count);
        Assert.IsType<List<Book>>(books);
    }

    [Theory]
    [InlineData("978-3-16-148410-0")] // Valid ISBN-13
    [InlineData("0-306-40615-2")] // Valid ISBN-10
    [InlineData("9783161484100")] // Valid ISBN-13 without dashes
    [InlineData("0306406152")] // Valid ISBN-10 without dashes
    public void Book_WithValidISBN_ShouldPassValidation(string isbn)
    {
        var book = CreateBook(isbn);

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(book, new ValidationContext(book), validationResults, true);

        Assert.True(isValid);
        Assert.Empty(validationResults);
    }

    [Theory]
    [InlineData(null)] // Null ISBN
    [InlineData("")] // Empty ISBN
    [InlineData("123")] // Too short
    [InlineData("978-3-16-148410-0123")] // Too long
    [InlineData("9783161484100123")] // Too long AND without dashes
    [InlineData("978-3-16-148410-X")] // Invalid character
    public void Book_WithInvalidISBN_ShouldFailValidation(string? isbn)
    {
        var book = CreateBook(isbn!);

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(book, new ValidationContext(book), validationResults, true);

        Assert.False(isValid);
        Assert.NotEmpty(validationResults);
    }
}
