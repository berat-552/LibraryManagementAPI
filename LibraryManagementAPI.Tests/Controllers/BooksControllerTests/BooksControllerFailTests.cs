using LibraryManagementAPI.Controllers;
using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Tests.Controllers.BooksControllerTests;

public class BooksControllerFailTests
{
    private readonly BooksController _controller;
    private readonly LibraryContext _context;
    private readonly ITestOutputHelper _output; // Debug purposes

    public BooksControllerFailTests(ITestOutputHelper output)
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
        _context.Books.AddRange(SeedData.SeedBooks());
        _context.SaveChanges();

        _controller = new BooksController(_context);
        _output = output;
    }

    [Fact]
    public async Task GetBooks_NoBooks_ReturnsNotFound()
    {
        // Clear the database to simulate no books
        _context.Books.RemoveRange(_context.Books);
        await _context.SaveChangesAsync();

        var response = await _controller.GetBooks();
        var notFoundResult = Assert.IsType<NotFoundResult>(response.Result);

        Assert.Null(response.Value);
        Assert.IsNotType<List<Book>>(response.Value);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }

    private const int InvalidBookId = 999;

    [Fact]
    public async Task GetBookById_InvalidId_ReturnsNotFound()
    {
        var response = await _controller.GetBookById(InvalidBookId);
        var notFoundResult = Assert.IsType<NotFoundResult>(response.Result);

        Assert.Null(response.Value);
        Assert.IsNotType<Book>(response.Value);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }

    [Theory]
    [InlineData(-3)]
    [InlineData(0)]
    [InlineData(-250)]
    public async Task GetBooksByQuantity_InvalidQuantity_ReturnsBadRequest(int invalidQuantity)
    {
        var response = await _controller.GetBooksByQuantity(invalidQuantity);
        var badRequestResult = Assert.IsType<BadRequestResult>(response.Result);

        Assert.Null(response.Value);
        Assert.IsNotType<List<Book>>(response.Value);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task UpdateBook_InvalidFields_ShouldReturnBadRequest()
    {
        var bookToUpdate = SeedData.SeedBooks().First();
        bookToUpdate.BookTitle = "";

        _controller.ModelState.AddModelError("bookTitle", "bookTitle is required");
        _controller.ModelState.AddModelError("bookTitle", "bookTitle must be between 1 and 100 characters");

        var response = await _controller.UpdateBook(bookToUpdate.Id, bookToUpdate);
        var badRequestResult = Assert.IsType<BadRequestResult>(response);

        Assert.NotNull(badRequestResult);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task DeleteBook_BookDoesntExist_ShouldReturnNotFound()
    {
        var response = await _controller.DeleteBook(InvalidBookId);
        var notFoundResult = Assert.IsType<NotFoundResult>(response);

        Assert.NotNull(notFoundResult);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }
}
