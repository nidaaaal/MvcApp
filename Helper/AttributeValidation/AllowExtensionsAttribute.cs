using System.ComponentModel.DataAnnotations;

namespace MvcApp.Helper.AttributeValidation
{
    public class AllowExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extension;

        public AllowExtensionsAttribute(string[] extension)
        {
            _extension = extension;

        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();

                if (!_extension.Contains(extension))
                {
                    return new ValidationResult($"Allowed types :{string.Join(",",_extension)} ");

                }

            }
            return ValidationResult.Success;

        }
    }
}
