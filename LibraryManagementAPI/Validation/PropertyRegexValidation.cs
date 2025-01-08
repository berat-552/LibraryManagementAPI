using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace LibraryManagementAPI.Validation;

public sealed class PropertyRegexValidation(string regexPattern) : ValidationAttribute
{
    private readonly Regex _regex = new(regexPattern, RegexOptions.Compiled);

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null) return ValidationResult.Success; // Consider null as valid. Use [Required] for mandatory fields.
        if (value is not string propertyValue) return new ValidationResult($"The field {validationContext.DisplayName} must be a string.");

        return _regex.IsMatch(propertyValue) ? ValidationResult.Success : new ValidationResult($"The field {validationContext.DisplayName} is not in the correct format.");
    }
}