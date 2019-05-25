using System.Collections.Generic;
using System.Threading.Tasks;
using GlodnyStudent.Data;
using GlodnyStudent.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GlodnyStudent.Models.Repositories.Implementations
{
    public class RestaurantRepository : IRestaurantRepository
    {
        public async Task<Restaurant> Create(Restaurant restaurant)
        {
            Restaurant result = null;

            using (var context = new ApplicationDbContext())
            {
                result = context.Restaurants.Add(restaurant).Entity;
                await context.SaveChangesAsync();
            }
            return result;
        }

        public async Task Delete(long id)
        {
            using (var context = new ApplicationDbContext())
            {
                Restaurant result = await context.Restaurants.FirstOrDefaultAsync(restaurant => restaurant.Id == id);

                context.Entry(result).State = EntityState.Deleted;

                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Restaurant>> FindAll()
        {
            var result = new List<Restaurant>();

            using (var context = new ApplicationDbContext())
            {
                result = await context.Restaurants.ToListAsync();
            }
            return result;
        }

        public async Task<Restaurant> FindById(long id)
        {
            Restaurant result = null;

            using (var context = new ApplicationDbContext())
            {
                result = await context.Restaurants.FirstOrDefaultAsync(restaurant => restaurant.Id == id);
            }
            return result;
        }

        public async Task<Restaurant> Update(Restaurant restaurant)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(restaurant).State = EntityState.Modified;

                await context.SaveChangesAsync();
            }
            return restaurant;
        }
    }
}
