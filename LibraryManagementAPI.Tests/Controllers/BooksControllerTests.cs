using LibraryManagementAPI.Controllers;
using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace LibraryManagementAPI.Tests.Controllers;

public class BooksControllerTests
{
    private readonly BooksController _controller;
    private readonly LibraryContext _context;
    private readonly ITestOutputHelper _output; // Debug purposes

    public BooksControllerTests(ITestOutputHelper output)
    {
        var dbName = Guid.NewGuid().ToString();

        // Configure in memory database options
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        // Initialize the database context with the in-memory options
        _context = new LibraryContext(options);

        // Clear the database before seeding it with test data
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        // Seed the in-memory database with test data
        _context.Books.AddRange(Book.GetTestBooks());
        _context.SaveChanges();

        _controller = new BooksController(_context);
        _output = output;
    }

    [Fact]
    public async Task GetBooks_ReturnsAllBooks()
    {
        var totalBooksCount = 3;
        var result = await _controller.GetBooks();
        var books = Assert.IsType<List<Book>>(result.Value);

        Assert.NotEmpty(books);
        Assert.Equal(totalBooksCount, books.Count);
    }

    [Fact]
    public async Task GetBookById_ReturnsBook()
    {
        var id = 1;
        var result = await _controller.GetBookById(id);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var foundBook = Assert.IsType<Book>(okResult.Value);

        Assert.NotNull(foundBook);
        Assert.Equal(id, foundBook.Id);
        Assert.NotEmpty(_context.Books);
    }

    [Fact]
    public async Task CreateBook_ReturnsBook()
    {
        var bookToCreate = new Book
        {
            BookTitle = "The Great Adventure",
            ISBN = "123-4567890123",
            Genre = "Fantasy",
            PublishedDate = new DateTime(2020, 5, 15),
            AuthorId = 42
        };

        var result = await _controller.CreateBook(bookToCreate);
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdBook = Assert.IsType<Book>(createdResult.Value);

        Assert.NotNull(createdBook);
        Assert.Equal(bookToCreate.BookTitle, createdBook.BookTitle);
        Assert.Equal(bookToCreate.Genre, createdBook.Genre);
        Assert.Equal(bookToCreate.PublishedDate, createdBook.PublishedDate);
        Assert.Equal(bookToCreate.ISBN, createdBook.ISBN);
    }

    [Fact]
    public async Task UpdateBook_ReturnsOkResult_WhenBookIsUpdatedBook()
    {
        var bookToUpdate = Book.GetTestBooks().First();

        bookToUpdate.Genre = "Action";

        var result = await _controller.UpdateBook(bookToUpdate.Id, bookToUpdate);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var updatedBook = Assert.IsType<Book>(okResult.Value);

        Assert.Equal(bookToUpdate.Id, updatedBook.Id);
        Assert.Equal(bookToUpdate.Genre, updatedBook.Genre);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
    }

    [Fact]
    public async Task DeleteBook_ReturnsNoContentResult_WhenBookIsDeleted()
    {
        var bookToDelete = Book.GetTestBooks().First();
        var result = await _controller.DeleteBook(bookToDelete.Id);
        var noContentResult = Assert.IsType<NoContentResult>(result);
        var deletedBook = await _context.Books.FindAsync(bookToDelete.Id);

        Assert.Null(deletedBook);
        Assert.NotNull(noContentResult);
        Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
    }
}
