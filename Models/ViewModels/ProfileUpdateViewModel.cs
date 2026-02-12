using MvcApp.Helper;
using MvcApp.Helper.Validation;
using System.ComponentModel.DataAnnotations;

namespace MvcApp.Models.ViewModels
{
    public class ProfileUpdateViewModel
    {
        [Required]
        [MinLength(3), MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]

        [MaxLength(50)]

        public string LastName { get; set; } = null!;

        [MaxLength(50)]
        public string? DisplayName { get; set; }

        [Required]
        [DateOfBirth]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public int Gender { get; set; }

        [Required]
        [MaxLength(500)]
        public string Address { get; set; } = null!;

        [MaxLength(50)]
        public string? City { get; set; } = null;

        [MaxLength(50)]
        public string? State { get; set; } = null;

        [Required]
        [Range(100000, 999999)]
        public int ZipCode { get; set; }

        [Required]
        [MaxLength(10)]
        public string Phone { get; set; } = null!;
        [MaxLength(10)]
        public string? Mobile { get; set; } = null;
    }
}
