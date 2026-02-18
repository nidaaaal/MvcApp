using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcApp.Models.ViewModels
{
    public class UsersViewModel
    {
        [Required]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("first_name")]

        public string? FirstName {get;set;}

        [Required]
        [Column("last_name")]

        public string? LastName { get; set; }

        [Required]
        [Column("phone")]

        public string? Phone { get; set; }

        [Required]
        [Column("role")]

        public string? Role { get; set; }

    }
}
