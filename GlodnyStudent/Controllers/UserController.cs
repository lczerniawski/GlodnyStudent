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
    //[Authorize]
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
                    return BadRequest("Podany użytkownik nie istnieje");

                user.Password = _authService.HashPassword(resetPasswordViewModel.NewPassword);

                var result = _userRepository.Update(user);
                if (result == null)
                    return BadRequest("Nie udało się zmienić hasła");

                return Ok("Hasło zmienione");
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
                    return BadRequest("Podany użytkownik nie istnieje");

                if (_authService.VerifyPassword(user.Password,
                    _authService.HashPassword(changePasswordViewModel.OldPassword)))
                {
                    user.Password = _authService.HashPassword(changePasswordViewModel.NewPassword);
                    var result = _userRepository.Update(user);

                    if (result == null)
                        return BadRequest("Błąd podczas zmiany hasła");

                    return Ok("Hasło zmienione poprawnie");
                }
                else
                {
                    return BadRequest("Błędne stare hasło");
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
                    return NotFound("Nie ma takiego użytkownika");

                var resetToken = _authService.GetAuthData(getUser.Id, getUser.Username, getUser.Role);

                await _emailSender.SendEmailAsync(email,"Reset Hasła GłodnyStudent","Wygląda na to, że prosiłeś o zmiane hasła. Aby jej dokonac przejdź pod link: https://localhost:44375/ResetHasla/ "+ getUser.Username + "/" + resetToken.Token);

                return Ok("Mail wysłany");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
            
        }

    }
}