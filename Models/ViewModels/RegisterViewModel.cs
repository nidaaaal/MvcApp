using MvcApp.Helper;
using System.ComponentModel.DataAnnotations;

namespace MvcApp.Models.ViewModels
{
    public class RegisterViewModel
    {
            [Required]
            [MaxLength(255)]
            public string UserName { get; set; } = null!;

            [Required]
            public string Password { get; set; } = null!;

            [Required]
            [Compare(nameof(Password))]
            public string ConfirmPassword { get; set; } = null!;

            [Required]
            [MinLength(3),MaxLength(50)]
            public string FirstName { get; set; } = null!;

            [Required]
            [MaxLength(50)]

            public string LastName { get; set; } = null!;

            [MaxLength(50)]
            public string? DisplayName { get; set; }

            [Required]
            public int Day { get; set; }

            [Required]
            public int Month { get; set; }

            [Required]
            public int Year { get; set; }
            
            public int Age { get; set; }

            [Required]
            public bool Gender { get; set; }

            [Required, MinLength(10), MaxLength(255)]
            public string Address { get; set; } = null!;
            
            [MaxLength(50)]
            public string? City { get; set; } = null;

            [MaxLength(50)]
            public string? State { get; set; } = null;

            [Required]
            [Range(100000,999999)]
            public int ZipCode { get; set; }

            [Required, StringLength(10, MinimumLength = 10)]
            public string Phone { get; set; }= null!;

            [ StringLength(10, MinimumLength = 10)]
            public string? Mobile { get; set; } = null;
    }
}
