using System.Collections.Generic;
using System.Threading.Tasks;
using GlodnyStudent.Data;
using GlodnyStudent.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GlodnyStudent.Models.Repositories.Implementations
{
    public class CuisineRepository : ICuisineRepository
    {
        public async Task<Cuisine> Create(Cuisine cuisine)
        {
            Cuisine result = null;

            using (var context = new ApplicationDbContext())
            {
                result = context.Cuisines.Add(cuisine).Entity;
                await context.SaveChangesAsync();
            }
            return result;
        }

        public async Task Delete(string name)
        {
            using (var context = new ApplicationDbContext())
            {
                Cuisine result = await context.Cuisines.FirstOrDefaultAsync(cuisine => cuisine.Name == name);

                context.Entry(result).State = EntityState.Deleted;

                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Cuisine>> FindAll()
        {
            var result = new List<Cuisine>();

            using (var context = new ApplicationDbContext())
            {
                result = await context.Cuisines.ToListAsync();
            }
            return result;
        }

        public async Task<Cuisine> FindById(string name)
        {
            Cuisine result = null;

            using (var context = new ApplicationDbContext())
            {
                result = await context.Cuisines.FirstOrDefaultAsync(cuisine => cuisine.Name == name);
            }
            return result;
        }

        public async Task<Cuisine> Update(Cuisine cuisine)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(cuisine).State = EntityState.Modified;

                await context.SaveChangesAsync();
            }
            return cuisine;
        }
    }
}
