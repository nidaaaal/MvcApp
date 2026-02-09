using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MvcApp.Helper.Validation
{
    public class EmailOrPhoneAttribute : ValidationAttribute, IClientModelValidator
    {
        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add(
                "data-val-emailorphone",
                ErrorMessage ?? "Enter a valid email or 10-digit phone number."
            );
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            var input = value.ToString();

            var isEmail = Regex.IsMatch(input, @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$");
            var isPhone = Regex.IsMatch(input, @"^\d{10}$");

            if (isEmail || isPhone)
                return ValidationResult.Success;

            return new ValidationResult("Enter a valid email or 10-digit phone number.");
        }
    }
}
