using System.ComponentModel.DataAnnotations;

namespace MvcApp.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [MaxLength(255)]
        public string UserName { get; set; } = null!;
        [Required]
        [MaxLength(50)]
        public string Password { get; set; } = null!;
    }
}
