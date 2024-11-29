using LibraryManagementAPI.Helpers;
using LibraryManagementAPI.Models;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Tests.Helpers;

public class AuthenticationHandlerTests
{
    private readonly IConfiguration _configuration;

    public AuthenticationHandlerTests()
    {
        // Set up in-memory configuration with a valid JWT secret (appsettings.json)
        var inMemorySettings = new Dictionary<string, string> {
            {"JWT_Secret", "SuperSecretKeyThatIsAtLeast32CharactersLong"}
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();
    }

    [Fact]
    public void GenerateJWTToken_ShouldReturnValidToken()
    {
        var user = new LibraryMember
        {
            Id = 1,
            Username = "testuser",
            Email = "testuser@example.com",
            Password = "securePassword123"
        };

        var authHandler = new AuthenticationHandler(_configuration);
        var token = authHandler.GenerateJWTToken(user);
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);

        Assert.False(string.IsNullOrEmpty(token));
        Assert.NotNull(jwtToken);

        Assert.Equal(user.Id.ToString(), jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        Assert.Equal(user.Username, jwtToken.Claims.First(c => c.Type == ClaimTypes.Name).Value);
    }

    [Fact]
    public void GenerateJWTToken_ShouldThrowException_WhenSecretIsInvalid()
    {
        // Set up in-memory configuration with invalid JWT key (too short)
        var inMemorySettings = new Dictionary<string, string> {
            {"JWT_Secret", "short"}
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        var user = new LibraryMember
        {
            Id = 1,
            Username = "testuser",
            Email = "testuser@example.com",
            Password = "securePassword123"
        };

        var authHandler = new AuthenticationHandler(configuration);

        Assert.Throws<InvalidOperationException>(() => authHandler.GenerateJWTToken(user));
    }
}