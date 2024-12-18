using LibraryManagementAPI.Controllers;
using LibraryManagementAPI.Data;
using LibraryManagementAPI.Helpers;
using LibraryManagementAPI.Models;
using LibraryManagementAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Xunit.Abstractions;

namespace Tests.Controllers.LibraryMembersControllerTests;

public class LibraryMembersControllerFailTests
{
    private readonly LibraryMembersController _controller;
    private readonly LibraryMemberService _libraryMemberService;
    private readonly AppDbContext _context;
    private readonly ITestOutputHelper _output; // Debug purposes
    private readonly IConfiguration _configuration;

    public LibraryMembersControllerFailTests(ITestOutputHelper output)
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

        _libraryMemberService = new LibraryMemberService(_context, authenticationHandler);
        _controller = new LibraryMembersController(_libraryMemberService);
        _output = output;
    }

    [Fact]
    public async Task RegisterLibraryMember_ExistingMember_ReturnsConflict()
    {
        var memberToCreate = SeedData.SeedLibraryMembers().First();
        var response = await _controller.RegisterLibraryMember(memberToCreate);
        var conflictResult = Assert.IsType<ConflictResult>(response.Result);

        Assert.NotNull(response);
        Assert.Equal(StatusCodes.Status409Conflict, conflictResult.StatusCode);
    }

    [Fact]
    public async Task GetLibraryMemberById_ReturnsForbid_WhenNotAuthorized()
    {
        var memberId = 1;
        var unauthorizedUserId = 2;
        var user = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, unauthorizedUserId.ToString())
        ], "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var result = await _controller.GetLibraryMemberById(memberId);

        Assert.IsType<ForbidResult>(result.Result);
    }

    [Fact]
    public async Task GetLibraryMemberById_WhenMemberDoesNotExist_ReturnsNotFound()
    {
        var memberId = 1;
        var user = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, memberId.ToString())
        ], "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var response = await _controller.GetLibraryMemberById(memberId);
        var notFoundResult = Assert.IsType<NotFoundResult>(response.Result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task LoginLibraryMember_ReturnsUnauthorized_WhenCredentialsAreInvalid()
    {
        var loginModel = new LoginModel
        {
            Email = "invaliduser@example.com",
            Password = "wrongPassword"
        };

        var response = await _controller.LoginLibraryMember(loginModel);
        var unauthorizedResult = Assert.IsType<UnauthorizedResult>(response);
        Assert.Equal(StatusCodes.Status401Unauthorized, unauthorizedResult.StatusCode);
    }

    [Fact]
    public async Task LoginLibraryMember_ReturnsUnauthorized_WhenPasswordIsIncorrect()
    {
        var loginModel = new LoginModel
        {
            Email = "testuser@example.com",
            Password = "wrongPassword"
        };

        var response = await _controller.LoginLibraryMember(loginModel);
        var unauthorizedResult = Assert.IsType<UnauthorizedResult>(response);
        Assert.Equal(StatusCodes.Status401Unauthorized, unauthorizedResult.StatusCode);
    }

    [Fact]
    public async Task UpdateLibraryMember_ReturnsForbid_WhenNotAuthorized()
    {
        var memberId = 74;
        var unauthorizedUserId = 2;
        var user = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, unauthorizedUserId.ToString())
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

        Assert.IsType<ForbidResult>(response.Result);
    }

    [Fact]
    public async Task UpdateLibraryMember_ReturnsNotFound_WhenMemberDoesNotExist()
    {
        var nonExistentMemberId = 999;
        var user = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, nonExistentMemberId.ToString())
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

        var response = await _controller.UpdateLibraryMember(nonExistentMemberId, updatedMember);

        var notFoundResult = Assert.IsType<NotFoundResult>(response.Result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task DeleteLibraryMember_ReturnsForbid_WhenNotAuthorized()
    {
        var memberId = 74;
        var unauthorizedUserId = 75; // Different user ID
        var user = new ClaimsPrincipal(new ClaimsIdentity(
        [
        new Claim(ClaimTypes.NameIdentifier, unauthorizedUserId.ToString())
        ], "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var response = await _controller.DeleteLibraryMember(memberId);
        Assert.IsType<ForbidResult>(response);
    }

    [Fact]
    public async Task DeleteLibraryMember_ReturnsNotFound_WhenMemberDoesNotExist()
    {
        var nonExistentMemberId = 999;
        var user = new ClaimsPrincipal(new ClaimsIdentity(
        [
        new Claim(ClaimTypes.NameIdentifier, nonExistentMemberId.ToString())
        ], "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var response = await _controller.DeleteLibraryMember(nonExistentMemberId);

        var notFoundResult = Assert.IsType<NotFoundResult>(response);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }
}