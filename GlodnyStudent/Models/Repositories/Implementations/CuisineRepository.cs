using System.Collections.Generic;
using System.Linq;
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

        public async Task Delete(long id)
        {
            Cuisine result = await _context.Cuisines.FirstOrDefaultAsync(cuisine => cuisine.Id == id);

            _context.Entry(result).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        public async Task<Cuisine[]> FindAll()
        {
            return await _context.Cuisines.ToArrayAsync();
        }

        public async Task<Cuisine> FindById(long id)
        {
            return await _context.Cuisines.FirstOrDefaultAsync(cuisine => cuisine.Id == id);
        }

        public async Task<Cuisine> Update(Cuisine cuisine)
        {
            _context.Entry(cuisine).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return cuisine;
        }
    }
}
