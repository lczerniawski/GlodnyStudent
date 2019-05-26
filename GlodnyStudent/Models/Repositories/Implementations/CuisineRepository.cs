using System.Collections.Generic;
using System.Threading.Tasks;
using GlodnyStudent.Data;
using GlodnyStudent.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GlodnyStudent.Models.Repositories.Implementations
{
    public class CuisineRepository : ICuisineRepository
    {
        private readonly ApplicationDbContext _context;

        public CuisineRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cuisine> Create(Cuisine cuisine)
        {
            Cuisine result = _context.Cuisines.Add(cuisine).Entity;

            await _context.SaveChangesAsync();

            return result;
        }

        public async Task Delete(string name)
        {
            Cuisine result = await _context.Cuisines.FirstOrDefaultAsync(cuisine => cuisine.Name == name);

            _context.Entry(result).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Cuisine>> FindAll()
        {
            return await _context.Cuisines.ToListAsync();
        }

        public async Task<Cuisine> FindByName(string name)
        {
            return await _context.Cuisines.FirstOrDefaultAsync(cuisine => cuisine.Name == name); ;
        }

        public async Task<Cuisine> Update(Cuisine cuisine)
        {
            _context.Entry(cuisine).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return cuisine;
        }
    }
}
