using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlodnyStudent.Models.Domain
{
    public class MenuItem
    {
        public long Id { get; set; }

        [Required]
        [RegularExpression(@"^[\p{Lu}\p{Ll} ]*$", ErrorMessage = "Name is invalid")]
        public string Name { get; set; }

        public decimal Price { get; set; }

        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }

        public Restaurant Restaurant { get; set; }
    }
}
