using LibraryManagementAPI.Interfaces;
using LibraryManagementAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.Models;

public class LibraryMember : ILibraryMember
{
    public Guid Id { get; set; } = new Guid();

    [Required(ErrorMessage = "userName is required")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "username must be between 1 and 100 characters")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "email is required")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "email must be between 1 and 100 characters")]
    [Email]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "password is required")]
    [StringLength(50, MinimumLength = 8, ErrorMessage = "password must be between 1 and 50 characters")]
    public string Password { get; set; } = string.Empty;
}
