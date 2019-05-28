using GlodnyStudent.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlodnyStudent.Models.Repositories
{
    public interface IReviewRepository : ICrudRepository<Review>
    {
        Task<IEnumerable<Review>> FindAllByRestaurantId(long restaurantId);
    }
}
