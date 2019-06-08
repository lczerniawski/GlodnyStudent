using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GlodnyStudent.Data.Abstract;
using GlodnyStudent.Models;
using GlodnyStudent.Models.Domain;
using GlodnyStudent.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GlodnyStudent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UsersManagerController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersManagerController(IUserRepository userRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<UserViewModel[]>> ListAllUsers([FromQuery]string startsWith)
        {
            try
            {
                var users = await _userRepository.FindAll();
                if (users == null)
                    return BadRequest("Błąd podczas pobierania użytkowników");

                List<UserViewModel> usersList = new List<UserViewModel>();

                foreach (var user in users)
                {
                    if (user.Username.ToLower().StartsWith(startsWith.ToLower()))
                    {
                        usersList.Add(new UserViewModel
                        {
                            Username = user.Username,
                            Status = user.Status.ToString()
                        });
                    }
                }

                return usersList.ToArray();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserViewModel>> BanUnbanUser([FromBody]string username)
        {
            try
            {
                var user = await _userRepository.FindUserByUsername(username);
                if (user == null)
                    return NotFound("Nie ma takiego użytkownika");

                if (user.Status == StatusType.Active)
                    user.Status = StatusType.Banned;
                else
                    user.Status = StatusType.Active;

                var updatedUser = await _userRepository.Update(user);
                if (updatedUser == null)
                    return BadRequest("Błąd podczas blokowania użytkownika");

                UserViewModel result = new UserViewModel
                {
                    Username = updatedUser.Username,
                    Status = updatedUser.Status.ToString()
                };

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }
    }
}