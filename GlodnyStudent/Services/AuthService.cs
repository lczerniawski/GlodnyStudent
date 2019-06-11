using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GlodnyStudent.ViewModels;
using CryptoHelper;
using GlodnyStudent.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace GlodnyStudent.Services
{
    public class AuthService : IAuthService
    {
        string jwtSecret;
        int jwtLifespan;
        /// <summary>
        /// Tworzy obiekt odpowiadający za generowanie tokenów wykorzystywanych w celach authoryzacji użytkowników
        /// </summary>
        /// <param name="jwtSecret">Sekretny klucz służący generowaniu tokena</param>
        /// <param name="jwtLifespan">Czas ważnośći wygenerowanego tokenu</param>
        public AuthService(string jwtSecret, int jwtLifespan)
        {
            this.jwtSecret = jwtSecret;
            this.jwtLifespan = jwtLifespan;
        }
        /// <summary>
        /// Funkcja odpowiadająca za wytwarzanie i zwracanie obiektu który słuzy do obsługi autoryzacji klienta.
        /// </summary>
        /// <param name="id">Id użytkownika</param>
        /// <param name="username">Nazwa użytkownika</param>
        /// <param name="role">Rola użytkownika w aplikacji</param>
        /// <returns></returns>
        public AuthData GetAuthData(string id,string username,RoleType role)
        {
            var expirationTime = DateTime.UtcNow.AddSeconds(jwtLifespan);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, id),
                    new Claim(ClaimTypes.Role,role.ToString())
                }),
                Expires = expirationTime,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            return new AuthData
            {
                Token = token,
                TokenExpirationTime = ((DateTimeOffset)expirationTime).ToUnixTimeSeconds(),
                Id = id,
                Username = username,
                Role = role.ToString(),
                Status = StatusCodes.Status200OK
            };
        }

        /// <summary>
        /// Funkcja haszująca hasło
        /// </summary>
        /// <param name="password">Hasło podane jako plain text</param>
        /// <returns></returns>
        public string HashPassword(string password)
        {
            return Crypto.HashPassword(password);
        }

        /// <summary>
        /// Funkcja służąca do sprawdzenia poprawności wprowadzonego przez użytkownika hasła
        /// </summary>
        /// <param name="actualPassword">Hasło podane w plain tekscie</param>
        /// <param name="hashedPassword">Haszowane hasło które jest przecyhowywane np w bazie danych</param>
        /// <returns></returns>
        public bool VerifyPassword(string actualPassword, string hashedPassword)
        {
            return Crypto.VerifyHashedPassword(hashedPassword, actualPassword);
        }
    }
}