using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlodnyStudent.Data;
using GlodnyStudent.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GlodnyStudent.Models.Repositories.Implementations
{
    public class RestaurantAddressRepository : IRestaurantAddressRepository
    {
        private readonly ApplicationDbContext _context;

        public RestaurantAddressRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RestaurantAddress> Create(RestaurantAddress address)
        {
            RestaurantAddress result = _context.RestaurantAddresses.Add(address).Entity;

            await _context.SaveChangesAsync();

            return result;
        }

        public async Task Delete(long id)
        {
            RestaurantAddress result = await _context.RestaurantAddresses
                    .FirstOrDefaultAsync(address => address.Id == id);

            _context.Entry(result).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<RestaurantAddress>> FindAll()
        {
            return await _context.RestaurantAddresses.ToListAsync();
        }

        public async Task<RestaurantAddress> FindById(long id)
        {
            return await _context.RestaurantAddresses
                .FirstOrDefaultAsync(address => address.Id == id);
        }

        public async Task<RestaurantAddress> Update(RestaurantAddress address)
        {
            _context.Entry(address).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return address;
        }
    }
}
