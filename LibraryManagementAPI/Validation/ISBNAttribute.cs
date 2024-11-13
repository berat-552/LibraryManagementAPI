using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace LibraryManagementAPI.Validation
{
    public class ISBNAttribute : ValidationAttribute
    {
        private const string Pattern = @"^(?=(?:[^0-9]*[0-9]){10}(?:(?:[^0-9]*[0-9]){3})?$)[\d-]+$";

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success; // Consider null as valid. Use [Required] for mandatory fields.
            }

            // if it is a string, assigns it to the variable isbn
            if (value is not string isbn)
            {
                return new ValidationResult("Invalid ISBN format.");
            }

            var regex = new Regex(Pattern);
            if (regex.IsMatch(isbn))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid ISBN format.");
        }
    }
}
