using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace LibraryManagementAPI.Validation;

public partial class EmailAttribute : ValidationAttribute
{
    private const string Pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

    [GeneratedRegex(Pattern)]
    private static partial Regex MyRegex();
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null) return ValidationResult.Success; // Consider null as valid. Use [Required] for mandatory fields.
        if (value is not string email) return new ValidationResult("Invalid email format.");

        var regex = MyRegex();
        return regex.IsMatch(email) ? ValidationResult.Success : new ValidationResult("Invalid email format.");
    }
}
