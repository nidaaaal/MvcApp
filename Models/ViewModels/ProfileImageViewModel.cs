using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MvcApp.Models.ViewModels
{

    public class ProfileImageViewModel
    {
        [Required]

        public IFormFile file { get; set; } = null!;

        [Required]

        public string ExstingUrl { get; set; } = string.Empty;

        [Required]

        public bool Preview { get; set; }

    }
}
