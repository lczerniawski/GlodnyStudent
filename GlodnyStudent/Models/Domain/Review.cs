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
        
        public string UserId { get; set; }

        public virtual User User { get; set; }

        public long RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Review review &&
                   Id == review.Id &&
                   AddTime == review.AddTime &&
                   UserId == review.UserId &&
                   RestaurantId == review.RestaurantId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, AddTime, RestaurantId);
        }
    }
}
