using System.Collections.Generic;
using System.Threading.Tasks;
using GlodnyStudent.Data;
using GlodnyStudent.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GlodnyStudent.Models.Repositories.Implementations
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly ApplicationDbContext _context;

        public RestaurantRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Restaurant> Create(Restaurant restaurant)
        {
            Restaurant result = _context.Restaurants.Add(restaurant).Entity;

            await _context.SaveChangesAsync();

            return result;
        }

        public async Task Delete(long id)
        {
            Restaurant result = await _context.Restaurants.FirstOrDefaultAsync(restaurant => restaurant.Id == id);

            _context.Entry(result).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Restaurant>> FindAll()
        {
            return await _context.Restaurants.ToListAsync(); ;
        }

        public async Task<Restaurant> FindById(long id)
        {
            return await _context.Restaurants
                .FirstOrDefaultAsync(restaurant => restaurant.Id == id);
        }

        public async Task<Restaurant> Update(Restaurant restaurant)
        {
            _context.Entry(restaurant).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return restaurant;
        }
    }
}
