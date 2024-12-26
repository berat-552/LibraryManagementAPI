using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace LibraryManagementAPI.Validation;

public partial class ISBNAttribute : ValidationAttribute
{
    private const string Pattern = @"^(?=(?:[^0-9]*[0-9]){10}(?:(?:[^0-9]*[0-9]){3})?$)[\d-]+$";

    [GeneratedRegex(Pattern)]
    private static partial Regex MyRegex();

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null) return ValidationResult.Success; // Consider null as valid. Use [Required] for mandatory fields.
        if (value is not string isbn) return new ValidationResult("Invalid ISBN format."); // if it is a string, assigns it to the variable isbn

        var regex = MyRegex();
        return regex.IsMatch(isbn) ? ValidationResult.Success : new ValidationResult("Invalid ISBN format.");
    }
}