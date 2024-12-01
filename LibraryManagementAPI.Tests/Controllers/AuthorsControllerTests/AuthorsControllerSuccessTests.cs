using LibraryManagementAPI.Controllers;
using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Tests.Controllers.AuthorsControllerTests;

public class AuthorsControllerSuccessTests
{
    private readonly AuthorsController _controller;
    private readonly LibraryContext _context;
    private readonly ITestOutputHelper _output; // Debug purposes

    public AuthorsControllerSuccessTests(ITestOutputHelper output)
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
        _context.Authors.AddRange(SeedData.SeedAuthors());
        _context.SaveChanges();

        // Initialize the controller with the test database context
        _controller = new AuthorsController(_context);
        _output = output;
    }

    [Fact]
    public async Task GetAuthors_ReturnsAllAuthors()
    {
        var totalAuthorsCount = 9;
        var response = await _controller.GetAuthors();
        var okResult = Assert.IsType<OkObjectResult>(response.Result);
        var authors = Assert.IsType<List<Author>>(okResult.Value);

        Assert.NotEmpty(authors);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.Equal(totalAuthorsCount, authors.Count);
    }

    [Fact]
    public async Task GetAuthorsByQuantity_ReturnsWantedQuantityOfAuthors()
    {
        var quantity = 4;
        var response = await _controller.GetAuthorsByQuantity(quantity);

        var okResult = Assert.IsType<OkObjectResult>(response.Result);
        var authors = Assert.IsType<List<Author>>(okResult.Value);

        Assert.NotEmpty(authors);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.Equal(quantity, authors.Count);
    }

    [Fact]
    public async Task GetAuthorsByQuantity_ReturnsPartialContent()
    {
        var quantity = 30;
        var response = await _controller.GetAuthorsByQuantity(quantity);

        var okResult = Assert.IsType<ObjectResult>(response.Result);
        var authors = Assert.IsType<List<Author>>(okResult.Value);

        Assert.NotEmpty(authors);
        Assert.Equal(StatusCodes.Status206PartialContent, okResult.StatusCode);
        Assert.NotEqual(quantity, authors.Count);
    }

    [Fact]
    public async Task GetAuthorById_ReturnsAuthor()
    {
        var id = 31;
        var response = await _controller.GetAuthorById(id);

        var okResult = Assert.IsType<OkObjectResult>(response.Result);
        var foundAuthor = Assert.IsType<Author>(okResult.Value);

        Assert.NotNull(foundAuthor);
        Assert.Equal(id, foundAuthor.Id);
    }

    [Fact]
    public async Task CreateAuthor_ReturnsAuthor()
    {
        var authorToCreate = new Author
        {
            AuthorName = "John Doe",
            Biography = "John Doe is a prolific author known for his engaging novels."
        };

        var response = await _controller.CreateAuthor(authorToCreate);
        var createdResult = Assert.IsType<CreatedAtActionResult>(response.Result);
        var createdAuthor = Assert.IsType<Author>(createdResult.Value);

        Assert.NotNull(createdAuthor);
        Assert.Equal(authorToCreate.AuthorName, createdAuthor.AuthorName);
        Assert.Equal(authorToCreate.Biography, createdAuthor.Biography);
    }

    [Fact]
    public async Task UpdateAuthor_ReturnsOkResult_WhenAuthorIsUpdated()
    {
        var authorToUpdate = SeedData.SeedAuthors().First();

        authorToUpdate.AuthorName = "Revised Author Name";

        var response = await _controller.UpdateAuthor(authorToUpdate.Id, authorToUpdate);
        var okResult = Assert.IsType<OkObjectResult>(response);
        var updatedAuthor = Assert.IsType<Author>(okResult.Value);

        Assert.Equal(authorToUpdate.Id, updatedAuthor.Id);
        Assert.Equal(authorToUpdate.AuthorName, updatedAuthor.AuthorName);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
    }

    [Fact]
    public async Task DeleteAuthor_ReturnsNoContentResult_WhenAuthorIsDeleted()
    {
        var authorToDelete = SeedData.SeedAuthors().First();

        var response = await _controller.DeleteAuthor(authorToDelete.Id);
        var noContentResult = Assert.IsType<NoContentResult>(response);
        var deletedAuthor = await _context.Authors.FindAsync(authorToDelete.Id);

        Assert.NotNull(noContentResult);
        Assert.Null(deletedAuthor);
        Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
    }
}