using System.Threading.Tasks;
using GlodnyStudent.Models.Domain;

namespace GlodnyStudent.Models.Repositories
{
    public interface IRestaurantRepository : ICrudRepository<Restaurant>
    {
        Task<Restaurant[]> GetRestaurantsByStreet(string address);
    }
}
