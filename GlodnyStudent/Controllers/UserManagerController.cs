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
    //[Authorize(Roles = "Admin")]
    public class UserManagerController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserManagerController(IUserRepository userRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<string[]>> ListAllUsers([FromBody]string startsWith)
        {
            try
            {
                var users = await _userRepository.FindAll();
                if (users == null)
                    return BadRequest("Błąd podczas pobierania użytkowników");

                List<string> usersList = new List<string>();

                foreach (var user in users)
                {
                    if(user.Username.ToLower().StartsWith(startsWith.ToLower()))
                        usersList.Add(user.Username);
                }

                return usersList.ToArray();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> BanUser([FromBody]string username)
        {
            try
            {
                var user = await _userRepository.FindUserByUsername(username);
                if (user == null)
                    return NotFound("Nie ma takiego użytkownika");

                user.Status = StatusType.Banned;

                var result = await _userRepository.Update(user);
                if (result == null)
                    return BadRequest("Błąd podczas blokowania użytkownika");

                return true;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure!");
            }
        }
    }
}