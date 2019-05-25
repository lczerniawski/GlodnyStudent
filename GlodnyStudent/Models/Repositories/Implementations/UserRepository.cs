using System.Collections.Generic;
using System.Threading.Tasks;
using GlodnyStudent.Data;
using GlodnyStudent.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GlodnyStudent.Models.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        public async Task<User> Create(User user)
        {
            User result = null;

            using (var context = new ApplicationDbContext())
            {
                result = context.Users.Add(user).Entity;
                await context.SaveChangesAsync();
            }
            return result;
        }

        public async Task Delete(long id)
        {
            using (var context = new ApplicationDbContext())
            {
                User result = await context.Users.FirstOrDefaultAsync(user => user.Id == id);

                context.Entry(result).State = EntityState.Deleted;

                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> FindAll()
        {
            var result = new List<User>();

            using (var context = new ApplicationDbContext())
            {
                result = await context.Users.ToListAsync();
            }
            return result;
        }

        public async Task<User> FindById(long id)
        {
            User result = null;

            using (var context = new ApplicationDbContext())
            {
                result = await context.Users.FirstOrDefaultAsync(user => user.Id == id);
            }
            return result;
        }

        public async Task<User> Update(User user)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(user).State = EntityState.Modified;

                await context.SaveChangesAsync();
            }
            return user;
        }
    }
}
