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
        public async Task<Review> Create(Review review)
        {
            Review result = null;

            using (var context = new ApplicationDbContext())
            {
                result = context.Reviews.Add(review).Entity;
                await context.SaveChangesAsync();
            }
            return result;
        }

        public async Task Delete(long id)
        {
            using (var context = new ApplicationDbContext())
            {
                Review result = await context.Reviews
                    .FirstOrDefaultAsync(review => review.Id == id);

                context.Entry(result).State = EntityState.Deleted;

                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Review>> FindAll()
        {
            var result = new List<Review>();

            using (var context = new ApplicationDbContext())
            {
                result = await context.Reviews.ToListAsync();
            }
            return result;
        }

        public async Task<IEnumerable<Review>> FindAllByRestaurantId(long restaurantId)
        {
            var result = new List<Review>();

            using (var context = new ApplicationDbContext())
            {
                result = await context.Reviews
                    .Where(image => image.RestaurantId == restaurantId)
                    .ToListAsync();
            }
            return result;
        }

        public async Task<Review> FindById(long id)
        {
            Review result = null;

            using (var context = new ApplicationDbContext())
            {
                result = await context.Reviews.FirstOrDefaultAsync(review => review.Id == id);
            }
            return result;
        }

        public async Task<Review> Update(Review review)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(review).State = EntityState.Modified;

                await context.SaveChangesAsync();
            }
            return review;
        }
    }
}
