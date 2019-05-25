using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlodnyStudent.Data;
using GlodnyStudent.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GlodnyStudent.Models.Repositories.Implementations
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Review> Create(Review review)
        {
            Review result = _context.Reviews.Add(review).Entity;

            await _context.SaveChangesAsync();

            return result;
        }

        public async Task Delete(long id)
        {
            Review result = await _context.Reviews
                    .FirstOrDefaultAsync(review => review.Id == id);

            _context.Entry(result).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Review>> FindAll()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task<IEnumerable<Review>> FindAllByRestaurantId(long restaurantId)
        {
            return await _context.Reviews
                    .Where(image => image.RestaurantId == restaurantId)
                    .ToListAsync();
        }

        public async Task<Review> FindById(long id)
        {
            return await _context.Reviews
                .FirstOrDefaultAsync(review => review.Id == id);
        }

        public async Task<Review> Update(Review review)
        {
            _context.Entry(review).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return review;
        }
    }
}
