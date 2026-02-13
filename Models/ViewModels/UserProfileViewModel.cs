using System.ComponentModel.DataAnnotations;

namespace MvcApp.Models.ViewModels
{
    public class UserProfileViewModel
    {
        [Required]
        [MinLength(3), MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required] [MaxLength(50)]
        public string LastName { get; set; } = null!;

        [MaxLength(50)]

        public string? DisplayName { get; set; }

        [Required]

        [RegularExpression("^(Male|Female|male|female)$", ErrorMessage = "Gender must be 'Male' or 'Female'.")]
        public string Gender { get; set; } = null!;

        [Required]
        public int GenderCode { get; set; }

        public string DateOfBirth { get; set; } = null!;

        [Range(1, 31)]
        public int Day { get; set; }

        [Range(1, 12)]
        public int Month { get; set; }

        [Range(1900, 2026)]
        public int Year { get; set; }

        [Range(13,99)]
        public int Age { get; set; }

        [Required]
        [MaxLength(500)]
        public string Address { get; set; } = null!;

        [MaxLength(50)]
        public string? City { get; set; }

        [MaxLength(50)]
        public string? State { get; set; }

        [Required]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Enter a valid 6-digit ZIP code")]
        public int ZipCode { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Enter a valid 10-digit phone number")]
        public string Phone { get; set; } = null!;

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Enter a valid 10-digit phone number")]
        public string? Mobile { get; set; } = null;

        public string? ProfilePath { get; set; }    

    }

}
