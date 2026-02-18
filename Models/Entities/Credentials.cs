using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcApp.Models.Entities
{
    [Table("credentials", Schema = "auth")]
    public class Credential
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("username")]

        [MaxLength(255)]
        public string UserName { get; set; } = string.Empty;

        [Column("hashed_password")]
        [Required]
        [MaxLength(225)]
        public string HashedPassword { get; set; } = string.Empty;

        [Column("role")]
        public string? Role { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("login_at")]
        public DateTime? LoginAt { get; set; }
    }
}