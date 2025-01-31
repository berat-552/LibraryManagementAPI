﻿using LibraryManagementAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.Models;

public sealed class Book
{
    public int Id { get; set; }

    [Required(ErrorMessage = "title is required")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "bookTitle must be between 1 and 100 characters")]
    public string BookTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "isbn is required")]
    [PropertyRegexValidation(RegexPatterns.Isbn)]
    public string Isbn { get; set; } = string.Empty; // ISBN - International Standard Book Number

    [Required(ErrorMessage = "genre is required")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "genre must be between 1 and 100 characters")]
    public string Genre { get; set; } = string.Empty;

    [Required(ErrorMessage = "publishedDate is required")]
    public DateTime PublishedDate { get; set; }

    [Required(ErrorMessage = "authorId is required")]
    public int AuthorId { get; set; }
}