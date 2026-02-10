using System.ComponentModel.DataAnnotations;

namespace MvcApp.Models.ViewModels
{
    public class UserProfileViewModel
    {
        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        public string? DisplayName { get; set; }

        [Required]

        public string Gender { get; set; } = null!;

        public string DateOfBirth { get; set; } = null!;


        public int Age { get; set; }

        [Required]
        public string Address { get; set; } = null!;

        public string? City { get; set; }

        public string? State { get; set; }

        [Required]
        public int ZipCode { get; set; }

        [Required]
        public string Phone { get; set; } = null!;

        public string? Mobile { get; set; }

        public bool IsEditing { get; set; }
    }

}
