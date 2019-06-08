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
        bool IsUsernameUniq(string username);
        bool isEmailUniq(string email);
        User FindUserByEmail(string email);

        User FindUserByUsername(string email);

        Task<User> Create(User user);

        Task Delete(string id);

        Task<IEnumerable<User>> FindAll();

        Task<User> FindById(string id);

        Task<User> Update(User user);
    }
}
