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
        /// <summary>
        /// Metoda Login znajduje użytkownika na podstawie danych logowania. Na podstawie emailu podanego w DTO sprawdza czy użytkownik istnieje,jeżeli tak to sprawdza czy podane haslo jest prawidłowe i czy użytkownik ma uprawnienia do korzystania z aplikacji.
        /// Odwołanie do API następuje po adresie "nazwahosta/api/Auth/login" metodą POST w żądaniu należy zawrzeć odpowiednie pola które opisuje klasa LoginViewModel
        /// </summary>
        /// <param name="model">Obiekt klasy LoginViewModel czyli DTO formularza logowania.</param>
        /// <returns>
        /// W przypadku podania złego adresu Email użykownika: Status Code: 400 oraz wiadomość "Podany użytkownik nie istnieje"\n
        /// W przypadku podania złego  hasła: Status Code: 400 oraz wiadomość "Błędne hasło"\n
        /// W przypadku powodzenia: Status Code: 200, ID, Token, ważność tokenu role. \n
        /// W przypadku blędu modelu: Odpowiedni status code, oraz wiadomość z błędem
        /// </returns>
        [HttpPost("login")]
        public async Task<ActionResult<AuthData>> Login([FromBody]LoginViewModel model)
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

        /// <summary>
        /// Metoda Register tworzy użytkownika na podstawie danych podanych w formularzu rejestracji.
        /// Odwołanie do API następuje po adresie "nazwahosta/api/Auth/register" metodą POST w żądaniu należy zawrzeć odpowiednie pola które opisuje klasa RegisterViewModel
        /// </summary>
        /// <param name="model">Obiekt klasy RegisterViewModel czyli DTO formularza rejestracji.</param>
        /// <returns>
        /// W przypadku błędu modelu: jego status\n
        /// W przypadku użykownika zarajestrowanego pod podanym adresem email: Status Code: 400 oraz wiadomość "Użytkownik z podanym adresem email już istnieje"\n
        /// W przypadku użykownika zarajestrowanego pod podaną nazwą:  Status Code: 400 oraz wiadomość "Użytkownik z podaną nazwą już istnieje"\n
        /// W przypadku powodzenia: Status Code: 200, ID, Token, ważność tokenu role. \n
        /// </returns>
        [HttpPost("register")]
        public async Task<ActionResult<AuthData>> Register([FromBody]RegisterViewModel model)
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