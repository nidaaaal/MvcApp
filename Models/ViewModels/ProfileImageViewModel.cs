using Microsoft.AspNetCore.Mvc;
using MvcApp.Helper.AttributeValidation;
using System.ComponentModel.DataAnnotations;

namespace MvcApp.Models.ViewModels
{

    public class ProfileImageViewModel
    {
        [Required(ErrorMessage ="Please upload an image")]
        [AllowExtensions(new[] {".jpg",".jpeg",".png"} )]
        [FileSize(2*1024*1024)]
        public IFormFile File { get; set; } = null!;

        public string? ExistingImagePath { get; set; }
    }
}
