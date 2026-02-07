using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcApp.Models.Entities
{
    public class Users
    {
        [Column("id")]

        public int Id { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; } = null!;

        [Column("last_name")]
        public string LastName { get; set; } = null!;

        [Column("display_name")]

        public string? DisplayName { get; set; } 

        [Column("date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        [Column("gender")]
        public bool Gender { get; set; }
        [Column("age")]
        public int Age { get; set; }
        [Column("address")]
        public string Address { get; set; } = null!;
        [Column("city")]
        public string? City { get; set; }
        [Column("state")]
        public string? State { get; set; }
        [Column("zipcode")]

        public int ZipCode { get; set; }

        [Column("phone")]
        public string Phone { get; set; } = null!;

        [Column("mobile")]
        public string? Mobile { get; set; }
    }

}