using MvcApp.Helper.Validation;
using System.ComponentModel.DataAnnotations;

namespace MvcApp.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [MaxLength(255)]
        [RegularExpression(@"(^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$)|(^\d{10}$)",
            ErrorMessage = "Enter a valid email or 10-digit phone number")]
        public string UserName { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
