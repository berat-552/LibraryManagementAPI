using LibraryManagementAPI.Controllers;
using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using LibraryManagementAPI.Responses;
using LibraryManagementAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Tests.Controllers.BooksControllerTests;

public class BooksControllerSuccessTests
{
    private readonly BooksController _controller;
    private readonly BookService _bookService;
    private readonly AppDbContext _context;
    private readonly ITestOutputHelper _output; // Debug purposes

    public BooksControllerSuccessTests(ITestOutputHelper output)
    {
        var dbName = Guid.NewGuid().ToString();

        // Configure in memory database options
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        // Initialize the database context with the in-memory options
        _context = new AppDbContext(options);

        // Clear the database before seeding it with test data
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        // Seed the in-memory database with test data
        _context.Books.AddRange(SeedData.SeedBooks());
        _context.SaveChanges();

        _bookService = new BookService(_context);
        _controller = new BooksController(_bookService);
        _output = output;
    }

    [Fact]
    public async Task GetBooks_ReturnsAllBooks()
    {
        var totalBooksCount = 9;
        var response = await _controller.GetBooks();
        var okResult = Assert.IsType<OkObjectResult>(response.Result);
        var books = Assert.IsType<List<Book>>(okResult.Value);

        Assert.NotEmpty(books);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.Equal(totalBooksCount, books.Count);
    }

    [Fact]
    public async Task GetBooksByQuantity_ReturnsWantedQuantityOfAuthors()
    {
        var quantity = 4;
        var response = await _controller.GetBooksByQuantity(quantity);

        var okResult = Assert.IsType<OkObjectResult>(response.Result);
        var books = Assert.IsType<List<Book>>(okResult.Value);

        Assert.NotEmpty(books);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.Equal(quantity, books.Count);
    }

    [Fact]
    public async Task GetBooksByQuantity_ReturnsPartialContent()
    {
        var quantity = 30;
        var response = await _controller.GetBooksByQuantity(quantity);

        var okResult = Assert.IsType<ObjectResult>(response.Result);
        var responseObject = Assert.IsType<PartialResponse<Book>>(okResult.Value);

        Assert.NotEmpty(responseObject.Items);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.True(responseObject.PartialData);
        Assert.NotEqual(quantity, responseObject.Items.Count);
    }

    [Fact]
    public async Task GetBookById_ReturnsBook()
    {
        var id = 21;
        var response = await _controller.GetBookById(id);
        var okResult = Assert.IsType<OkObjectResult>(response.Result);
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
            Isbn = "123-4567892123",
            Genre = "Fantasy",
            PublishedDate = new DateTime(2020, 5, 15),
            AuthorId = 42
        };

        var response = await _controller.CreateBook(bookToCreate);
        var createdResult = Assert.IsType<CreatedAtActionResult>(response.Result);
        var createdBook = Assert.IsType<Book>(createdResult.Value);

        Assert.NotNull(createdBook);
        Assert.Equal(bookToCreate.BookTitle, createdBook.BookTitle);
        Assert.Equal(bookToCreate.Genre, createdBook.Genre);
        Assert.Equal(bookToCreate.PublishedDate, createdBook.PublishedDate);
        Assert.Equal(bookToCreate.Isbn, createdBook.Isbn);
    }

    [Fact]
    public async Task UpdateBook_ReturnsOkResult_WhenBookIsUpdatedBook()
    {
        var bookToUpdate = SeedData.SeedBooks().First();
        bookToUpdate.Genre = "Action";

        var response = await _controller.UpdateBook(bookToUpdate.Id, bookToUpdate);
        var okResult = Assert.IsType<OkObjectResult>(response);
        var updatedBook = Assert.IsType<Book>(okResult.Value);

        Assert.Equal(bookToUpdate.Id, updatedBook.Id);
        Assert.Equal(bookToUpdate.Genre, updatedBook.Genre);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
    }

    [Fact]
    public async Task DeleteBook_ReturnsNoContentResult_WhenBookIsDeleted()
    {
        var bookToDelete = SeedData.SeedBooks().First();
        var response = await _controller.DeleteBook(bookToDelete.Id);
        var noContentResult = Assert.IsType<NoContentResult>(response);
        var deletedBook = await _context.Books.FindAsync(bookToDelete.Id);

        Assert.Null(deletedBook);
        Assert.NotNull(noContentResult);
        Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
    }
}
