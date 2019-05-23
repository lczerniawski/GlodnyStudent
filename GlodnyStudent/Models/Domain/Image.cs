using System.ComponentModel.DataAnnotations.Schema;

namespace GlodnyStudent.Models.Domain
{
    public class Image
    {
        public long Id { get; set; }

        public byte[] ImageSource { get; set; }
        
        public long RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }
    }
}
