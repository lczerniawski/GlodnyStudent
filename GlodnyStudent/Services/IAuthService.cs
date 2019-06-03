using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlodnyStudent.ViewModels;
namespace GlodnyStudent.Services
{
    public interface IAuthService
    {
        AuthData GetAuthData(string id,string username);
        string HashPassword(string password);
        bool VerifyPassword(string actualPassword, string hashedPassword);
    }
}
