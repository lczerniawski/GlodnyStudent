using GlodnyStudent.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlodnyStudent.Models.Repositories
{
    interface IImageRepository : ICrudRepository<Image>
    {
        Task<IEnumerable<Image>> FindAllByRestaurantId(long restaurantId);
    }
}
