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
using GlodnyStudent.Models.Domain;
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
        public async Task<ActionResult<AuthData>> Post([FromBody]LoginViewModel model)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await userRepository.FindUserByEmail(model.Email);

            if (user == null)
            {
                return BadRequest(new{status = StatusCodes.Status400BadRequest, message = "Niepoprawny adres email"});
            }

            if (user.Status == StatusType.Banned)
                return BadRequest(new{status = StatusCodes.Status400BadRequest, message = "Zostałeś zbanowany"});


            var passwordValid = authService.VerifyPassword(model.Password, user.Password);
            if (!passwordValid)
            {
                return StatusCode(StatusCodes.Status409Conflict,new{status = StatusCodes.Status409Conflict, message = "Niepoprawne hasło"});
            }

            return authService.GetAuthData(user.Id,user.Username,user.Role);
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthData>> Post([FromBody]RegisterViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var emailUniq = await userRepository.isEmailUniq(model.Email);
            if (!emailUniq)
                return BadRequest(new{status = StatusCodes.Status400BadRequest, message = "Użytkownik o takim emailu już istnieje"});

            var usernameUniq = await userRepository.IsUsernameUniq(model.Username);
            if (!usernameUniq)
                return BadRequest(new{status = StatusCodes.Status400BadRequest, message = "Użytkownik o takiej nazwie już istnieje"});

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
                return StatusCode(StatusCodes.Status500InternalServerError,new{status = StatusCodes.Status500InternalServerError, message = "Tworzenie konta nie powiodło się"});


            return authService.GetAuthData(id,user.Username,user.Role);
        }

    }
}