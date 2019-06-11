using System.Collections.Generic;
using System.Threading.Tasks;
using GlodnyStudent.Data.Abstract;
using GlodnyStudent.Models;
using Microsoft.EntityFrameworkCore;

namespace GlodnyStudent.Data.Repositories
{
    /// <summary>
    /// Reprezentuje repozytorium użytkowników.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// Połączenia do bazy danych. 
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Konstruktor UserRepository przyjmuje jako argument połączenia do bazy danych.
        /// </summary>
        /// <param name="context">Podłączenia do bazy danych</param>
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Metoda isEmailUniq przyjmuje email, następnie sprawdza asynchronicznie czy dany parametr jest unikalny w tablicę baz danych - Users.
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>Operację asynchroniczną, która zwraca boolean</returns>
        public async Task<bool> isEmailUniq(string email)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            return user == null;
        }

        /// <summary>
        /// Metoda FindUserByEmail przyjmuje email, następnie szuka asynchronicznie danego użytkownika z tablicy baz danych - Users.
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>Operację asynchroniczną, która zwraca obiekt typu User</returns>
        public async Task<User> FindUserByEmail(string email)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            return user;
        }

        /// <summary>
        /// Metoda IsUsernameUniq przyjmuje username, następnie sprawdza asynchronicznie czy dany parametr jest unikalny w tablicę baz danych - Users.
        /// </summary>
        /// <param name="username">Nazwa Użytkownika</param>
        /// <returns>Operację asynchroniczną, która zwraca prawdę lub fałsz.</returns>
        public async Task<bool> IsUsernameUniq(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
            return user == null;
        }

        /// <summary>
        /// Metoda FindUserByUsername przyjmuje username, następnie szuka asynchronicznie danego użytkownika z tablicy baz danych - Users.
        /// </summary>
        /// <param name="username">Nazwa Użytkownika</param>
        /// <returns>Operację asynchroniczną, która zwraca obiekt typu User</returns>
        public async Task<User> FindUserByUsername(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);

            return user;
        }

        /// <summary>
        /// Metoda Create przyjmuje obiekt typu User, następnie według argumentu, tworzy nowy rekord w tablicę baz danych - Users.
        /// </summary>
        /// <param name="user">Obiekt typu Review</param>
        /// <returns>Operację asynchroniczną, która zwraca obiekt typu User</returns>
        public async Task<User> Create(User user)
        {
            User result = _context.Users.Add(user).Entity;

            await _context.SaveChangesAsync();

            return result;
        }

        /// <summary>
        /// Metoda Delete przyjmuje id użytkownika i na jej podstawie asynchronicznie usuwa rekord z tablicy baz danych - Users.
        /// </summary>
        /// <param name="id">Klucz podstawowy użytkownika</param>
        /// <returns>Operację asynchroniczną</returns>
        public async Task Delete(string id)
        {
            User result = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);

            _context.Entry(result).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Metoda FindAll asynchronicznie zwraca listę wszystkich rekordów z tablicy baz danych - Users.
        /// </summary>
        /// <returns>Operację asynchroniczną, która zwraca listę obiektów typu Review</returns>
        public async Task<IEnumerable<User>> FindAll()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Metoda FindById przyjmuje id użytkowników i na jej podstawie szuka asynchronicznie danego użytkownika z tablicy baz danych - Users.
        /// </summary>
        /// <param name="id">Klucz podstawowy restauracji</param>
        /// <returns>Operację asynchroniczną, która zwraca listę obiektów typu User</returns>
        public async Task<User> FindById(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        /// <summary>
        /// Metoda Update przyjmuje obiekt typu User, następnie według argumentu, aktualizuje rekord z tablicy baz danych - Users.
        /// </summary>
        /// <param name="user">Obiekt typu User</param>
        /// <returns>Operację asynchroniczną, która zwraca obiekt typu User</returns>
        public async Task<User> Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return user;
        }
    }
}
