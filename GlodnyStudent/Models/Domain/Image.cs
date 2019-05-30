using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlodnyStudent.Models.Domain
{
    public class Image
    {
        public long Id { get; set; }

        public string FilePath { get; set; }
        
        public long RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Image image && Id == image.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
