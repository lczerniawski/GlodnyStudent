using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlodnyStudent.Models.Domain
{
    public class Review
    {
        public long Id { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        public DateTime AddTime { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        [ForeignKey("Restaurant")]
        public long RestaurantId { get; set; }

        public Restaurant Restaurant { get; set; }
    }
}
