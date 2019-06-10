using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using GlodnyStudent.Data.Abstract;
using GlodnyStudent.Models;
using GlodnyStudent.Services;
using GlodnyStudent.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;

namespace GlodnyStudent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IEmailSender _emailSender;
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public UserController(IEmailSender emailSender,IUserRepository userRepository,IAuthService authService)
        {
            _emailSender = emailSender;
            _userRepository = userRepository;
            _authService = authService;
        }

        [Route("reset")]
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            try
            {
                var user = await _userRepository.FindUserByUsername(resetPasswordViewModel.Username);
                if (user == null)
                    return BadRequest(new{status = StatusCodes.Status400BadRequest, message = "Podany użytkownik nie istnieje"});

                user.Password = _authService.HashPassword(resetPasswordViewModel.NewPassword);

                var result = _userRepository.Update(user);
                if (result == null)
                    return BadRequest(new{status = StatusCodes.Status400BadRequest, message = "Nie udało się zmienić hasła"});

                return Ok(new{status = StatusCodes.Status400BadRequest, message = "Hasło zmienione"});

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }

        }

        [Route("ChangePassword")]
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            try
            {
                var user = await _userRepository.FindUserByUsername(changePasswordViewModel.Username);
                if (user == null)
                    return BadRequest(new{status = StatusCodes.Status400BadRequest, message = "Podany użytkownik nie istnieje"});

                    if (_authService.VerifyPassword(changePasswordViewModel.OldPassword,user.Password))
                {
                    user.Password = _authService.HashPassword(changePasswordViewModel.NewPassword);
                    var result = _userRepository.Update(user);

                    if (result == null)
                        return BadRequest("Błąd podczas zmiany hasła");

                    return Ok(new{status = StatusCodes.Status200OK, message = "Hasło zmienione poprawnie"});
                }
                else
                {
                    return StatusCode(StatusCodes.Status409Conflict,new { status = StatusCodes.Status409Conflict,message = "Błędne stare hasło" });
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }

        }

        //[Route("sendactivemail/{email}")]
        //[HttpGet]
        //public dynamic SendActiveMail(string email)
        //{
        //    dynamic result;
        //    var getUser = userRepository.GetByEmail(email);
        //    if (getUser != null)
        //    {
        //        string sent = Utilities.ActiveMailAsync(email);
        //        result = new
        //        {
        //            code = ReturnCodes.DataGetSucceeded,
        //            data = sent,
        //        };
        //    }
        //    else
        //    {
        //        result = new
        //        {
        //            code = ReturnCodes.DataGetFailed,
        //            data = "Fail",
        //        };
        //    }
        //    return result;
        //}

        [Route("sendresetmail/{email}")]
        [HttpGet]
        public async Task<IActionResult> SendResetMail(string email)
        {
            try
            {
                var getUser = await _userRepository.FindUserByEmail(email);
                if (getUser == null)
                    return NotFound(new{status = StatusCodes.Status404NotFound, message = "Nie ma takiego użytkownika"});

                var resetToken = _authService.GetAuthData(getUser.Id, getUser.Username, getUser.Role);

                await _emailSender.SendEmailAsync(email, "Reset Hasła GłodnyStudent", "Wygląda na to, że prosiłeś o zmiane hasła. Aby jej dokonac przejdź pod link: https://www.localhost:44375/ResetHasła/" + getUser.Username + "/" + resetToken.Token);

                return Ok(new{status = StatusCodes.Status200OK, message = "Mail wysłany"});

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
            
        }

    }
}