﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlodnyStudent.Models.Domain
{
    public class Restaurant
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[\p{Lu}\p{Ll} ]*$", ErrorMessage = "Name is invalid")]
        public string Name { get; set; }

        public virtual Cuisine CuisineType { get; set; }

        public virtual RestaurantAddress Address { get; set; }

        public virtual ICollection<MenuItem> Menu { get; set; }        

        public int Score { get; set; }

        [ForeignKey("User")]
        public int OwnerId { get; set; }

        public virtual User Owner { get; set; }

        public virtual ICollection<Image> Gallery { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}
