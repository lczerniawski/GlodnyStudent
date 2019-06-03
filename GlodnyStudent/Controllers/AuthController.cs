using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CryptoHelper;
using GlodnyStudent.Services;
using GlodnyStudent.ViewModels;
using GlodnyStudent.Data;
using GlodnyStudent.Data.Repositories;
using GlodnyStudent.Data.Abstract;
using GlodnyStudent.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GlodnyStudent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IAuthService authService;
        IUserRepository userRepository;
        public AuthController(IAuthService authService, IUserRepository userRepository)
        {
            this.authService = authService;
            this.userRepository = userRepository;
        }

        [HttpPost("login")]
        public ActionResult<AuthData> Post([FromBody]LoginViewModel model)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = userRepository.FindUserByEmail(model.Email);

            if (user == null)
            {
                return BadRequest(new { email = "no user with this email" });
            }

            var passwordValid = authService.VerifyPassword(model.Password, user.Password);
            if (!passwordValid)
            {
                return BadRequest(new { password = "invalid password" });
            }

            return authService.GetAuthData(user.Id,user.Username,user.Role);
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthData>> Post([FromBody]RegisterViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var emailUniq = userRepository.isEmailUniq(model.Email);
            if (!emailUniq) return BadRequest(new { email = "user with this email already exists" });
            var usernameUniq = userRepository.IsUsernameUniq(model.Username);
            if (!usernameUniq) return BadRequest(new { username = "user with this email already exists" });

            var id = Guid.NewGuid().ToString();
            var user = new User
            {
                Id = id,
                Username = model.Username,
                Email = model.Email,
                Password = authService.HashPassword(model.Password)
            };
            var userCreated = await userRepository.Create(user);

            if (userCreated == null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            return authService.GetAuthData(id,user.Username,user.Role);
        }

    }
}