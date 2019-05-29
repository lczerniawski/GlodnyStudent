using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlodnyStudent.Models;

namespace GlodnyStudent.Data.Abstract
{
    public interface IUserRepository : IEntityBaseRepository<User>
    {
        bool IsUsernameUniq(string username);
        bool isEmailUniq(string email);
    }
}
