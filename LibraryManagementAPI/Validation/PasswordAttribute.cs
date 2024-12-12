using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace LibraryManagementAPI.Validation;

public class PasswordAttribute : ValidationAttribute
{
    private const string Pattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*(),.?"":{}|<>])[A-Za-z\d!@#$%^&*(),.?"":{}|<>]{8,}$";

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success; // Consider null as valid. Use [Required] for mandatory fields.
        }

        if (value is not string password)
        {
            return new ValidationResult("Password must contain at least 8 characters, one special character, one uppercase letter, one lowercase letter and one number.");
        }

        var regex = new Regex(Pattern);
        if (regex.IsMatch(password))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult("Password must contain at least 8 characters, one special character, one uppercase letter, one lowercase letter and one number.");
    }
}