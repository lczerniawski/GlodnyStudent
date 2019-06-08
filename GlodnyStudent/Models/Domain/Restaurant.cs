using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlodnyStudent.Models.Domain
{
    public class Restaurant
    {
        public long Id { get; set; }

        [Required]
        [RegularExpression(@"^[\p{Lu}\p{Ll} ]*$", ErrorMessage = "Name is invalid")]
        public string Name { get; set; }

        public virtual Cuisine Cuisine { get; set; }

        public virtual RestaurantAddress Address { get; set; }

        public virtual ICollection<MenuItem> Menu { get; set; }        

        public int Score { get; set; }
        
        [ForeignKey("User")]
        public string OwnerId { get; set; }
        
        public virtual User Owner { get; set; }
        

        public virtual ICollection<Image> Gallery { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public int ReviewsCount { get; set; }

        [Column(TypeName = "decimal(13,2)")]
        public decimal HighestPrice { get; set; }

        public bool GotOwner { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Restaurant restaurant &&
                   Id == restaurant.Id &&
                   Name == restaurant.Name &&
                   EqualityComparer<RestaurantAddress>.Default.Equals(Address, restaurant.Address);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Address);
        }
    }
}
