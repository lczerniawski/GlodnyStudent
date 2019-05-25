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
        public async Task<MenuItem> Create(MenuItem menuItem)
        {
            MenuItem result = null;

            using (var context = new ApplicationDbContext())
            {
                result = context.MenuItems.Add(menuItem).Entity;
                await context.SaveChangesAsync();
            }
            return result;
        }

        public async Task Delete(long id)
        {
            using (var context = new ApplicationDbContext())
            {
                MenuItem result = await context.MenuItems.FirstOrDefaultAsync(item => item.Id == id);

                context.Entry(result).State = EntityState.Deleted;

                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<MenuItem>> FindAll()
        {
            var result = new List<MenuItem>();

            using (var context = new ApplicationDbContext())
            {
                result = await context.MenuItems.ToListAsync();
            }
            return result;
        }

        public async Task<IEnumerable<MenuItem>> FindAllByRestaurantId(long restaurantId)
        {
            var result = new List<MenuItem>();

            using (var context = new ApplicationDbContext())
            {
                result = await context.MenuItems
                    .Where(item => item.RestaurantId == restaurantId)
                    .ToListAsync();
            }
            return result;
        }

        public async Task<MenuItem> FindById(long id)
        {
            MenuItem result = null;

            using (var context = new ApplicationDbContext())
            {
                result = await context.MenuItems.FirstOrDefaultAsync(item => item.Id == id);
            }
            return result;
        }

        public async Task<MenuItem> Update(MenuItem menuItem)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(menuItem).State = EntityState.Modified;

                await context.SaveChangesAsync();
            }
            return menuItem;
        }
    }
}
