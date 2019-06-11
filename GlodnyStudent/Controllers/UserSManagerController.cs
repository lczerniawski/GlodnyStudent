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

        /// <summary>
        /// Metoda ListAllUsers służy do tworzenia listy kont użytkowników o określonym początku ich nazwy, lista pobierana jest za pomocą repozytorium użytkownika. Wymaga praw administratora
        /// Odwołanie do API następuje po adresie "nazwahosta/api/UsersManager" metodą Get w żądaniu należy umieścić header Authorization z tokenem wygenerowanym poprzez logowanie
        /// </summary>
        /// <param name="startsWith">Poczatkowy ciag znaków na jakie ma zaczynac sie nazwa użytkownika</param>
        /// <returns>
        /// W przypadku nie znalezenia użytkowników ktorych nazwa zaczyna sie zadanym ciągiem: Status code 400 oraz wiadomość
        /// W przypadku powodzenia tablica zawierająca obiekty klasy UserViewModel
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<UserViewModel[]>> ListAllUsers([FromQuery]string startsWith)
        {
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

        /// <summary>
        /// Metoda BanUnbanUser służy do blokowania i odblokowywania kont użytkowników. Za pomocą repozytorium użytkownika sprawdza czy użytkownik istnieje. W przypadku znalezienia danego użytkownika zmienia status jego konta na zbanowane lub aktywne.
        /// Odwołanie do API następuje po adresie "nazwahosta/api/UsersManager" metodą Post w żądaniu należy umieścić body z nazwą użytkownika oraz header Authorization z tokenem wygenerowanym poprzez logowanie
        /// </summary>
        /// <param name="username">Nazwa użytkownika</param>
        /// <returns>
        /// W przypadku braku użykownika:Status code 400 oraz wiadomość "Nie ma takiego użytkownika"\n
        /// W przypadku błędu aktualizacji bazy danych:Status code 400 oraz wiadomość "Błąd podczas zmiany statusu użytkownika"\n
        /// W przypadku sukcesu: Status code 200 oraz nazwe uzytkownika i jego status
        /// W przypadku błędu dostępu do bazy danych: Database Failure!\n
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<UserViewModel>> BanUnbanUser([FromBody]string username)
        {
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
                    return BadRequest(new{status = StatusCodes.Status400BadRequest, message = "Błąd podczas zmiany statusu użytkownika"});


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