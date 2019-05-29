using GeoAPI.Geometries;
using GlodnyStudent.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlodnyStudent.Models.Repositories
{
    public interface IRestaurantAddressRepository : ICrudRepository<RestaurantAddress>
    {
        Task<IEnumerable<RestaurantAddress>> FindAllByDistance(int distance, IPoint userLocation);
    }
}
