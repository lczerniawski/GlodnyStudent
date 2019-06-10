using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlodnyStudent.Data;
using GlodnyStudent.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GlodnyStudent.Models.Repositories.Implementations
{
    /// <summary>
    /// Reprezentuje repozytorium restauracji.
    /// </summary>
    public class RestaurantRepository : IRestaurantRepository
    {
        /// <summary>
        /// Połączenia do bazy danych. 
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Konstruktor RestaurantRepository przyjmuje jako argument połączenia do bazy danych.
        /// </summary>
        /// <param name="context">Podłączenia do bazy danych</param>
        public RestaurantRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Metoda Create przyjmuje obiekt typu Restaurant, następnie według argumentu, tworzy nowy rekord w tablicę baz danych - Restaurants.
        /// </summary>
        /// <param name="restaurant">Obiekt typu Restaurant</param>
        /// <returns>Operację asynchroniczną, która zwraca obiekt typu Restaurant</returns>
        public async Task<Restaurant> Create(Restaurant restaurant)
        {
            Restaurant result = _context.Restaurants.Add(restaurant).Entity;

            await _context.SaveChangesAsync();

            return result;
        }

        /// <summary>
        /// Metoda Delete przyjmuje id restauracji i na jej podstawie asynchronicznie usuwa rekord z tablicy baz danych - Restaurants.
        /// </summary>
        /// <param name="id">Klucz podstawowy restauracji</param>
        /// <returns>Operację asynchroniczną</returns>
        public async Task Delete(long id)
        {
            Restaurant result = await _context.Restaurants.FirstOrDefaultAsync(restaurant => restaurant.Id == id);

            _context.Entry(result).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Metoda GetRestaurantsByStreet przyjmuje adres restauracji, następnie według argumentu asynchronicznie  
        /// szuka rekordy z tablicy baz danych - Restaurants, zwraca tablicę uzyskanych obiektów. 
        /// </summary>
        /// <param name="address">Adres restauracji</param>
        /// <returns>>Operację asynchroniczną, która zwraca tablicę obiektów typu Restaurant</returns>
        public async Task<Restaurant[]> GetRestaurantsByStreet(string address)
        {
            var query = from r in _context.Restaurants
                where r.Address.StreetName.ToLower() == address.ToLower()
                select r;

            return await query.ToArrayAsync();
        }

        /// <summary>
        /// Metoda FindAll asynchronicznie zwraca listę wszystkich rekordów z tablicy baz danych - Restaurants.
        /// </summary>
        /// <returns>Operację asynchroniczną, która zwraca listę obiektów typu Restaurant</returns>
        public async Task<IEnumerable<Restaurant>> FindAll()
        {
            return await _context.Restaurants.ToListAsync();
        }

        /// <summary>
        /// Metoda FindById przyjmuje id restauracji i na jej podstawie asynchronicznie zwraca 
        /// listę rekordów z tablicy baz danych - Restaurants.
        /// </summary>
        /// <param name="id">Klucz podstawowy restauracji</param>
        /// <returns>Operację asynchroniczną, która zwraca listę obiektów typu Restaurant</returns>
        public async Task<Restaurant> FindById(long id)
        {
            return await _context.Restaurants
                .FirstOrDefaultAsync(restaurant => restaurant.Id == id);
        }

        /// <summary>
        /// Metoda Update przyjmuje obiekt typu Restaurant, następnie według argumentu, aktualizuje rekord z tablicy baz danych - Restaurants.
        /// </summary>
        /// <param name="restaurant">Obiekt typu Restaurant</param>
        /// <returns>Operację asynchroniczną, która zwraca obiekt typu Restaurant</returns>
        public async Task<Restaurant> Update(Restaurant restaurant)
        {
            _context.Entry(restaurant).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return restaurant;
        }
    }
}
