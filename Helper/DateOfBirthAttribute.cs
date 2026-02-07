using System.ComponentModel.DataAnnotations;

namespace MvcApp.Helper
{

    public class DateOfBirthAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is DateTime date)
            {
                if (date.Date > DateTime.Today)
                {
                    return new ValidationResult("Date of birth cannot be in the future.");
                }

                if (date.Date < DateTime.Today.AddYears(-120))
                {
                    return new ValidationResult("Date of birth is not valid (too old).");
                }

                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid date format.");
        }
    }
}
