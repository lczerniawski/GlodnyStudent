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
            /**
            *  <summary>  
            *Metoda ListAllUsers służy do tworzenia listy kont użytkowników o określonym początku ich nazwy, lista pobierana jest za pomocą repozytorium użytkownika. 

            *</summary> 
            *<param name="startsWith">
            *string zawierający początek nazw użytkowników do pobrania. 
            *</param>
            *<returns>
            *
            *W przypadku braku użykownika: Błąd podczas pobierania użytkowników\n
            *W przypadku błędu dostępu do bazy danych: Database Failure!\n
            * 
            *</returns>
            */
            try
            {
                var users = await _userRepository.FindAll();
                if (users == null)
                    return BadRequest(new{status = StatusCodes.Status400BadRequest, message = "Błąd podczas pobierania użytkowników"});


                List<UserViewModel> usersList = new List<UserViewModel>();

                foreach (var user in users)
                {
                    if (user.Username.ToLower().StartsWith(startsWith.ToLower()))
                    {
                        usersList.Add(new UserViewModel
                        {
                            Username = user.Username,
                            UserStatus = user.Status.ToString(),
                            Status = StatusCodes.Status200OK
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
            /**
            *  <summary>  
            *Metoda BanUnbanUser służy do blokowania i odblokowywania kont użytkowników. Za pomocą repozytorium użytkownika sprawdza czy użytkownik istnieje. W przypadku znalezienia danego użytkownika zmienia status jego konta na zbanowane lub aktywne. 
            
            *</summary> 
            *<param name="username">
            * string zawierający nazwę użytkownika
            * </param>
            *<returns>
            *
            *W przypadku braku użykownika: Nie ma takiego użytkownika\n
            *W przypadku błędu aktualizacji bazy danych: Błąd podczas blokowania użytkownika\n
            *W przypadku błędu dostępu do bazy danych: Database Failure!\n
            * 
            *</returns>
            */
            try
            {
                var user = await _userRepository.FindUserByUsername(username);
                if (user == null)
                    return BadRequest(new{status = StatusCodes.Status400BadRequest, message = "Nie ma takiego użytkownika"});


                if (user.Status == StatusType.Active)
                    user.Status = StatusType.Banned;
                else
                    user.Status = StatusType.Active;

                var updatedUser = await _userRepository.Update(user);
                if (updatedUser == null)
                    return BadRequest(new{status = StatusCodes.Status400BadRequest, message = "Błąd podczas blokowania użytkownika"});


                UserViewModel result = new UserViewModel
                {
                    Username = updatedUser.Username,
                    UserStatus = updatedUser.Status.ToString(),
                    Status = StatusCodes.Status200OK
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