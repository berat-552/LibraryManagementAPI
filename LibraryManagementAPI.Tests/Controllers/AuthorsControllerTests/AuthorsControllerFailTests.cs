using LibraryManagementAPI.Controllers;
using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using LibraryManagementAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Tests.Controllers.AuthorsControllerTests;

public class AuthorsControllerFailTests
{
    private readonly AuthorsController _controller;
    private readonly AuthorService _authorService;
    private readonly AppDbContext _context;
    private readonly ITestOutputHelper _output; // Debug purposes

    public AuthorsControllerFailTests(ITestOutputHelper output)
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
        _context.Authors.AddRange(SeedData.SeedAuthors());
        _context.SaveChanges();

        _authorService = new AuthorService(_context);
        _controller = new AuthorsController(_authorService);
        _output = output;
    }

    private const int InvalidAuthorId = 999;

    [Fact]
    public async Task GetAuthors_NoAuthors_ShouldReturnNotFound()
    {
        // Clear the database to simulate no authors
        _context.Authors.RemoveRange(_context.Authors);
        await _context.SaveChangesAsync();

        var response = await _controller.GetAuthors();
        var notFoundResult = Assert.IsType<NotFoundResult>(response.Result);

        Assert.Null(response.Value);
        Assert.IsNotType<List<Author>>(response.Value);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task GetAuthorById_InvalidId_ShouldReturnNotFound()
    {
        var response = await _controller.GetAuthorById(InvalidAuthorId);
        var notFoundResult = Assert.IsType<NotFoundResult>(response.Result);

        Assert.Null(response.Value);
        Assert.IsNotType<Author>(response.Value);
        Assert.IsType<NotFoundResult>(notFoundResult);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }

    [Theory]
    [InlineData(-3)]
    [InlineData(0)]
    [InlineData(-250)]
    public async Task GetAuthorsByQuantity_InvalidQuantity_ReturnsBadRequest(int invalidQuantity)
    {
        var response = await _controller.GetAuthorsByQuantity(invalidQuantity);
        var badRequestResult = Assert.IsType<BadRequestResult>(response.Result);

        Assert.Null(response.Value);
        Assert.IsNotType<List<Author>>(response.Value);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task CreateAuthor_InvalidAuthorReturnsBadRequest()
    {
        var authorToCreate = new Author
        {
            Biography = "John Doe is a prolific author known for his engaging novels."
        };

        _controller.ModelState.AddModelError("AuthorName", "authorName is required");
        _controller.ModelState.AddModelError("AuthorName", "authorName must be between 1 and 100 characters");

        var response = await _controller.CreateAuthor(authorToCreate);
        var badRequestResult = Assert.IsType<BadRequestResult>(response.Result);

        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task UpdateAuthor_InvalidFields_ShouldReturnBadRequest()
    {
        var authorToUpdate = SeedData.SeedAuthors().First();
        authorToUpdate.AuthorName = "";

        _controller.ModelState.AddModelError("AuthorName", "authorName is required");
        _controller.ModelState.AddModelError("AuthorName", "authorName must be between 1 and 100 characters");

        var response = await _controller.UpdateAuthor(authorToUpdate.Id, authorToUpdate);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(response);

        Assert.NotNull(badRequestResult);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task DeleteAuthor_AuthorDoesntExist_ShouldReturnNotFound()
    {
        var response = await _controller.DeleteAuthor(InvalidAuthorId);
        var notFoundResult = Assert.IsType<NotFoundResult>(response);

        Assert.NotNull(notFoundResult);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }
}
