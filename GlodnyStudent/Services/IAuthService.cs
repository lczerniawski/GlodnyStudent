using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlodnyStudent.Models.Domain;
using GlodnyStudent.ViewModels;
namespace GlodnyStudent.Services
{
    public interface IAuthService
    {
        AuthData GetAuthData(string id,string username,RoleType role);
        string HashPassword(string password);
        bool VerifyPassword(string actualPassword, string hashedPassword);
    }
}
