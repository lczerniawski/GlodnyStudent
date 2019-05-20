using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlodnyStudent.Models.Domain
{
    public class RestaurantAddress
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Street is required")]
        [RegularExpression(@"^[\p{Lu}\p{Ll}0-9 ]*$", ErrorMessage = "Street is invalid")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Street number is required")]
        [MaxLength(7, ErrorMessage = "Street number is too long")]
        [RegularExpression(@"^[\p{Lu}\p{Ll}0-9 ]*$", ErrorMessage = "Street number is invalid")]
        public string StreetNumber { get; set; }
                
        public int LocalNumber { get; set; }

        [Required(ErrorMessage = "Postal code is required")]
        [MaxLength(6, ErrorMessage = "Postal code is too long")]
        [RegularExpression(@"[0-9]{2}-[0-9]{3}", ErrorMessage = "Postal code is invalid")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "City is required")]
        [MaxLength(7, ErrorMessage = "City is too long")]
        [RegularExpression(@"^[\p{Lu}\p{Ll}]*([ {1}]|[-{1}])?[\p{Lu}\p{Ll}]*$", ErrorMessage = "City is invalid")]
        public string City { get; set; }

        public virtual AddressCoordinates Coordinates { get; set; }

        [ForeignKey("Restaurant")]
        public long RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }
    }
}
