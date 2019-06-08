using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlodnyStudent.Data.Abstract;
using GlodnyStudent.Models;
using Microsoft.EntityFrameworkCore;

namespace GlodnyStudent.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
       
        public async Task<bool> isEmailUniq(string email)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            return user == null;
        }

        public async Task<User> FindUserByEmail(string email)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task<bool> IsUsernameUniq(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
            return user == null;
        }

        public async Task<User> FindUserByUsername(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);

            return user;
        }


        public async Task<User> Create(User user)
        {
            User result = _context.Users.Add(user).Entity;

            await _context.SaveChangesAsync();

            return result;
        }

        public async Task Delete(string id)
        {
            User result = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);

            _context.Entry(result).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> FindAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> FindById(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<User> Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return user;
        }
    }
}
