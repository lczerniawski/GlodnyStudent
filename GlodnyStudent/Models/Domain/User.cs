using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using GlodnyStudent.Models.Domain;

namespace GlodnyStudent.Models
{
    public class User
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Name is too long")]
        public string Username { get; set; }

        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Email is invalid")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        
        [Column(TypeName = "nvarchar(16)")]
        public RoleType Role { get; set; }
        
        [Column(TypeName = "nvarchar(16)")]
        public StatusType Status { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<Restaurant> Restaurants { get; set; }

        public override bool Equals(object obj)
        {
            return obj is User user &&
                   Id == user.Id &&
                   Username == user.Username &&
                   Email == user.Email;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Username, Email);
        }
    }
}