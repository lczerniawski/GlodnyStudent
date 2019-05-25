using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlodnyStudent.Models.Domain
{
    public class Cuisine
    {
        [Key]
        public string Name { get; set; }

        [ForeignKey("Restaurant")]
        public long RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Cuisine cuisine && Name == cuisine.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}
