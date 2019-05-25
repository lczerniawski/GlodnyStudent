using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlodnyStudent.Data;
using GlodnyStudent.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GlodnyStudent.Models.Repositories.Implementations
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly ApplicationDbContext _context;

        public MenuItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MenuItem> Create(MenuItem menuItem)
        {
            MenuItem result = _context.MenuItems.Add(menuItem).Entity;

            await _context.SaveChangesAsync();

            return result;
        }

        public async Task Delete(long id)
        {
            MenuItem result = await _context.MenuItems.FirstOrDefaultAsync(item => item.Id == id);

            _context.Entry(result).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MenuItem>> FindAll()
        {
            return await _context.MenuItems.ToListAsync();
        }

        public async Task<IEnumerable<MenuItem>> FindAllByRestaurantId(long restaurantId)
        {
            return await _context.MenuItems
                    .Where(item => item.RestaurantId == restaurantId)
                    .ToListAsync();
        }

        public async Task<MenuItem> FindById(long id)
        {
            return await _context.MenuItems.FirstOrDefaultAsync(item => item.Id == id);
        }

        public async Task<MenuItem> Update(MenuItem menuItem)
        {
            _context.Entry(menuItem).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return menuItem;
        }
    }
}
