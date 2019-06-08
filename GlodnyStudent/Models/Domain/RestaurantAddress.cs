using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlodnyStudent.Models.Domain
{
    public class RestaurantAddress
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Street is required")]
        [RegularExpression(@"^[\p{Lu}\p{Ll}0-9 ]*$", ErrorMessage = "Street is invalid")]
        public string StreetName { get; set; }

        [Required(ErrorMessage = "Street number is required")]
        [MaxLength(7, ErrorMessage = "Street number is too long")]
        [RegularExpression(@"^[\p{Lu}\p{Ll}0-9 ]*$", ErrorMessage = "Street number is invalid")]
        public string StreetNumber { get; set; }
                
        public int LocalNumber { get; set; }
        
        public IPoint Location { get; set; }

        public long RestaurantId { get; set; }

        public string District { get; set; }

        public virtual Restaurant Restaurant { get; set; }

        public override bool Equals(object obj)
        {
            return obj is RestaurantAddress address &&
                   Id == address.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
