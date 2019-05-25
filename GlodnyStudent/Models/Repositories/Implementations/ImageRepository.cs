using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlodnyStudent.Data;
using GlodnyStudent.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GlodnyStudent.Models.Repositories.Implementations
{
    public class ImageRepository : IImageRepository
    {
        public async Task<Image> Create(Image image)
        {
            Image result = null;

            using (var context = new ApplicationDbContext())
            {
                result = context.Images.Add(image).Entity;
                await context.SaveChangesAsync();
            }
            return result;
        }

        public async Task Delete(long id)
        {
            using (var context = new ApplicationDbContext())
            {
                Image result = await context.Images.FirstOrDefaultAsync(image => image.Id == id);

                context.Entry(result).State = EntityState.Deleted;

                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Image>> FindAll()
        {
            var result = new List<Image>();

            using (var context = new ApplicationDbContext())
            {
                result = await context.Images.ToListAsync();
            }
            return result;
        }

        public async Task<IEnumerable<Image>> FindAllByRestaurantId(long restaurantId)
        {
            var result = new List<Image>();

            using (var context = new ApplicationDbContext())
            {
                result = await context.Images
                    .Where(image => image.RestaurantId == restaurantId)
                    .ToListAsync();
            }
            return result;
        }

        public async Task<Image> FindById(long id)
        {
            Image result = null;

            using (var context = new ApplicationDbContext())
            {
                result = await context.Images.FirstOrDefaultAsync(image => image.Id == id);
            }
            return result;
        }

        public async Task<Image> Update(Image image)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(image).State = EntityState.Modified;

                await context.SaveChangesAsync();
            }
            return image;
        }
    }
}
