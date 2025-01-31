﻿using LibraryManagementAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.Models;

public sealed class LibraryMember
{
    public int Id { get; set; }

    [Required(ErrorMessage = "userName is required")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "username must be between 1 and 100 characters")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "email is required")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "email must be between 1 and 100 characters")]
    [PropertyRegexValidation(RegexPatterns.Email)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "password is required")]
    [PropertyRegexValidation(RegexPatterns.Password)]
    public string Password { get; set; } = string.Empty;
}