using System.ComponentModel.DataAnnotations.Schema;

namespace GlodnyStudent.Models.Domain
{
    public class AddressCoordinates
    {
        public long Id { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
        
        public long RestaurantAddressId { get; set; }

        public virtual RestaurantAddress RestaurantAddress { get; set; }
    }
}
