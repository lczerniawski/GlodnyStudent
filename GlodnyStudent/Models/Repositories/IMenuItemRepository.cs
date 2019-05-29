using GlodnyStudent.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlodnyStudent.Models.Repositories
{
    public interface IMenuItemRepository : ICrudRepository<MenuItem>
    {
        Task<IEnumerable<MenuItem>> FindAllByRestaurantId(long restaurantId);
    }
}
