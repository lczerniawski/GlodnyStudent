using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlodnyStudent.Models;
using GlodnyStudent.Models.Repositories;

namespace GlodnyStudent.Data.Abstract
{
    public interface IUserRepository
    {
        Task<bool> IsUsernameUniq(string username);
        Task<bool> isEmailUniq(string email);
        Task<User> FindUserByEmail(string email);

        Task<User> FindUserByUsername(string username);

        Task<User> Create(User user);

        Task Delete(string id);

        Task<IEnumerable<User>> FindAll();

        Task<User> FindById(string id);

        Task<User> Update(User user);
    }
}
