using System.ComponentModel.DataAnnotations;

namespace MvcApp.Helper.AttributeValidation
{
    public class FileSizeAttribute : ValidationAttribute
    {
        private readonly int _size;

        public FileSizeAttribute(int size) 
        {
            _size = size;
        }


        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is IFormFile file)
            {
               if( file.Length> _size)
                {
                    return new ValidationResult($"Maximum file size is : {_size/1024/1024} MB");
                }
            }

            return ValidationResult.Success;
        }
    }
}
