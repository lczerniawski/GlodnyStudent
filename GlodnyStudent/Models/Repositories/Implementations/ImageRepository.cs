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
        private readonly ApplicationDbContext _context;

        public ImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Image> Create(Image image)
        {
            Image result = _context.Images.Add(image).Entity;

            await _context.SaveChangesAsync();

            return result;
        }

        public async Task Delete(long id)
        {
            Image result = await _context.Images.FirstOrDefaultAsync(image => image.Id == id);

            _context.Entry(result).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Image>> FindAll()
        {
            return await _context.Images.ToListAsync(); ;
        }

        public async Task<IEnumerable<Image>> FindAllByRestaurantId(long restaurantId)
        {
            return await _context.Images
                    .Where(image => image.RestaurantId == restaurantId)
                    .ToListAsync();
        }

        public async Task<Image> FindById(long id)
        {
            return await _context.Images.FirstOrDefaultAsync(image => image.Id == id);
        }

        public async Task<Image> Update(Image image)
        {
            _context.Entry(image).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return image;
        }
    }
}
