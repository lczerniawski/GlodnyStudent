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
        public async Task<RestaurantAddress> Create(RestaurantAddress address)
        {
            RestaurantAddress result = null;

            using (var context = new ApplicationDbContext())
            {
                result = context.RestaurantAddresses.Add(address).Entity;
                await context.SaveChangesAsync();
            }
            return result;
        }

        public async Task Delete(long id)
        {
            using (var context = new ApplicationDbContext())
            {
                RestaurantAddress result = await context.RestaurantAddresses
                    .FirstOrDefaultAsync(address => address.Id == id);

                context.Entry(result).State = EntityState.Deleted;

                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<RestaurantAddress>> FindAll()
        {
            var result = new List<RestaurantAddress>();

            using (var context = new ApplicationDbContext())
            {
                result = await context.RestaurantAddresses.ToListAsync();
            }
            return result;
        }

        public async Task<RestaurantAddress> FindById(long id)
        {
            RestaurantAddress result = null;

            using (var context = new ApplicationDbContext())
            {
                result = await context.RestaurantAddresses
                    .FirstOrDefaultAsync(address => address.Id == id);
            }
            return result; ;
        }

        public async Task<RestaurantAddress> Update(RestaurantAddress address)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(address).State = EntityState.Modified;

                await context.SaveChangesAsync();
            }
            return address;
        }
    }
}
