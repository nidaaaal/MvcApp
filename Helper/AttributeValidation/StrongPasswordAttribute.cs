using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MvcApp.Helper.AttributeValidation
{
    public class StrongPasswordAttribute : ValidationAttribute, IClientModelValidator
    {
        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add(
                "data-val-strongpassword",
                ErrorMessage ?? "Password should contain."
            );
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var password = value.ToString();

            var isStrong = Regex.IsMatch(password,
                @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");

            if (isStrong)
                return ValidationResult.Success;

            return new ValidationResult(
                "Password must be at least 8 characters and contain uppercase, lowercase, number and special character.");
        }
    }
}
