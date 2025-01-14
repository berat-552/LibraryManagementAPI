using LibraryManagementAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.Models;

public sealed class LoginModel
{
    [Required(ErrorMessage = "Email is required")]
    [PropertyRegexValidation(RegexPatterns.Email, ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;
}