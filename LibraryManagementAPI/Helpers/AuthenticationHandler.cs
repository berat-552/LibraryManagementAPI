using LibraryManagementAPI.Interfaces;
using LibraryManagementAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryManagementAPI.Helpers;

public class AuthenticationHandler(IConfiguration configuration) : IAuthenticationHandler
{
    private readonly IConfiguration _configuration = configuration;

    public string GenerateJWTToken(LibraryMember user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email)
        };

        var jwtSecret = _configuration["JWT_Secret"];
        if (string.IsNullOrEmpty(jwtSecret) || jwtSecret.Length < 32)
        {
            throw new InvalidOperationException("JWT secret is not configured properly.");
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddDays(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}