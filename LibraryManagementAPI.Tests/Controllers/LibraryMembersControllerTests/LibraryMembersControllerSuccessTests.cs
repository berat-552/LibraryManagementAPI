using LibraryManagementAPI.Controllers;
using LibraryManagementAPI.Data;
using LibraryManagementAPI.Helpers;
using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Xunit.Abstractions;

namespace Tests.Controllers.LibraryMembersControllerTests;

public class LibraryMembersControllerSuccessTests
{
    private readonly LibraryMembersController _controller;
    private readonly LibraryContext _context;
    private readonly ITestOutputHelper _output; // Debug purposes
    private readonly IConfiguration _configuration;
    public LibraryMembersControllerSuccessTests(ITestOutputHelper output)
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
        _context.LibraryMembers.AddRange(SeedData.SeedLibraryMembers());
        _context.SaveChanges();

        // Set up in-memory configuration with a valid JWT secret (appsettings.json)
        var inMemorySettings = new Dictionary<string, string> {
            {"JWT_Secret", "SuperSecretKeyThatIsAtLeast32CharactersLong"}
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        var authenticationHandler = new AuthenticationHandler(_configuration);

        // Initialize the controller with the test database context
        _controller = new LibraryMembersController(_context, authenticationHandler);
        _output = output;
    }

    [Fact]
    public async Task RegisterLibraryMember_ReturnsLibraryMember()
    {
        var plainTextPassword = "passwordSecure3455";
        var memberToCreate = new LibraryMember
        {
            Username = "michaeljohnson675123",
            Email = "michaeljohnson@example.com",
            Password = plainTextPassword,
        };

        var response = await _controller.RegisterLibraryMember(memberToCreate);
        var createdResult = Assert.IsType<CreatedAtActionResult>(response.Result);
        var createdMember = Assert.IsType<LibraryMember>(createdResult.Value);

        Assert.NotNull(response);
        Assert.Equal(memberToCreate.Username, createdMember.Username);
        Assert.Equal(memberToCreate.Email, createdMember.Email);
        Assert.True(PasswordHandler.VerifyPassword(plainTextPassword, createdMember.Password));
        Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
    }

    [Fact]
    public async Task GetLibraryMemberById_ReturnsLibraryMember_WhenAuthorized()
    {
        var memberId = 74;
        var user = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, memberId.ToString())
        ], "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var response = await _controller.GetLibraryMemberById(memberId);

        var okResult = Assert.IsType<OkObjectResult>(response.Result);
        var returnedMember = Assert.IsType<LibraryMember>(okResult.Value);
        Assert.Equal(memberId, returnedMember.Id);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
    }

    [Fact]
    public async Task LoginLibraryMember_ReturnsOk_WithToken_WhenCredentialsAreValid()
    {
        var loginModel = new LoginModel
        {
            Email = "jane.doe@gmail.com",
            Password = "janeDoe123?"
        };

        var libraryMember = await _context.LibraryMembers.FirstOrDefaultAsync(member => member.Email == loginModel.Email);
        
        var response = await _controller.LoginLibraryMember(loginModel);
        var okResult = Assert.IsType<OkObjectResult>(response);
        Assert.IsType<string>(okResult.Value?.ToString());
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
    }

    [Fact]
    public async Task UpdateLibraryMember_ReturnsOk_WhenAuthorized()
    {
        var memberId = 74; 
        var user = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, memberId.ToString())
        ], "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var updatedMember = new LibraryMember
        {
            Username = "updatedUsername",
            Email = "updated@example.com",
            Password = "newSecurePassword123"
        };

        var response = await _controller.UpdateLibraryMember(memberId, updatedMember);

        var okResult = Assert.IsType<OkObjectResult>(response.Result);
        var returnedMember = Assert.IsType<LibraryMember>(okResult.Value);
        Assert.Equal(updatedMember.Username, returnedMember.Username);
        Assert.Equal(updatedMember.Email, returnedMember.Email);
        Assert.True(PasswordHandler.VerifyPassword(updatedMember.Password, returnedMember.Password));
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
    }

    [Fact]
    public async Task DeleteLibraryMember_Successful_ReturnsNoContent()
    {
        var id = 74;
        var user = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, id.ToString())
        ], "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var response = await _controller.DeleteLibraryMember(id);

        var noContentResult = Assert.IsType<NoContentResult>(response);
        Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
    }
}