using System.ComponentModel.DataAnnotations.Schema;

namespace GlodnyStudent.Models.Domain
{
    public class AddressCoordinates
    {
        public long Id { get; set; }

        public double Lat { get; set; }

        public double Lon { get; set; }

        [ForeignKey("RestaurantAddress")]
        public long RestaurantAddressId { get; set; }

        public virtual RestaurantAddress RestaurantAddress { get; set; }
    }
}
