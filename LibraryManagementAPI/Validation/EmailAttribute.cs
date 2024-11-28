using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace LibraryManagementAPI.Validation;

public class EmailAttribute : ValidationAttribute
{
    private const string Pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success; // Consider null as valid. Use [Required] for mandatory fields.
        }

        if (value is not string email)
        {
            return new ValidationResult("Invalid email format.");
        }

        var regex = new Regex(Pattern);
        if (regex.IsMatch(email))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult("Invalid email format.");
    }
}
