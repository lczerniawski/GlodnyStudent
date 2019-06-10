using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoAPI.Geometries;
using GlodnyStudent.Data;
using GlodnyStudent.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GlodnyStudent.Models.Repositories.Implementations
{
    /// <summary>
    /// Reprezentuje repozytorium adresów restauracji.
    /// </summary>
    public class RestaurantAddressRepository : IRestaurantAddressRepository
    {
        /// <summary>
        /// Połączenia do bazy danych. 
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Konstruktor RestaurantAddressRepository przyjmuje jako argument połączenia do bazy danych.
        /// </summary>
        /// <param name="context">Podłączenia do bazy danych</param>
        public RestaurantAddressRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Metoda Create przyjmuje obiekt typu RestaurantAddress, następnie według argumentu, tworzy nowy rekord w tablicę baz danych - RestaurantAddresses.
        /// </summary>
        /// <param name="address">Obiekt typu RestaurantAddress</param>
        /// <returns>Operację asynchroniczną, która zwraca obiekt typu RestaurantAddress</returns>
        public async Task<RestaurantAddress> Create(RestaurantAddress address)
        {
            RestaurantAddress result = _context.RestaurantAddresses.Add(address).Entity;

            await _context.SaveChangesAsync();

            return result;
        }

        /// <summary>
        /// Metoda Delete przyjmuje id adresu restauracji i na jej podstawie asynchronicznie usuwa rekord z tablicy baz danych - RestaurantAddresses.
        /// </summary>
        /// <param name="id">Klucz podstawowy adresu restauracji</param>
        /// <returns>Operację asynchroniczną</returns>
        public async Task Delete(long id)
        {
            RestaurantAddress result = await _context.RestaurantAddresses
                    .FirstOrDefaultAsync(address => address.Id == id);

            _context.Entry(result).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Metoda FindAll asynchronicznie zwraca listę wszystkich rekordów z tablicy baz danych - RestaurantAddresses.
        /// </summary>
        /// <returns>Operację asynchroniczną, która zwraca listę obiektów typu RestaurantAddress</returns>
        public async Task<IEnumerable<RestaurantAddress>> FindAll()
        {
            return await _context.RestaurantAddresses.ToListAsync();
        }

        /// <summary>
        /// Metoda FindAllByDistance przyjmuje dystans oraz lokalizacje użytkownika, następnie według tych argumentów asynchronicznie oblicza
        /// i szuka rekordy z tablicy baz danych - RestaurantAddresses, zwraca listę uzyskanych obiektów. 
        /// </summary>
        /// <param name="distance">Zakres dystansu</param>
        /// <param name="userLocation">Współrzędne geograficzne</param>
        /// <returns>>Operację asynchroniczną, która zwraca listę obiektów typu RestaurantAddress</returns>
        public async Task<IEnumerable<RestaurantAddress>> FindAllByDistance(int distance, IPoint userLocation)
        {
            return await _context.RestaurantAddresses
                .Where(address => address.Location.Distance(userLocation) <= distance)
                .ToListAsync();
        }

        /// <summary>
        /// Metoda FindById przyjmuje id adresu restauracji i na jej podstawie asynchronicznie zwraca 
        /// listę rekordów z tablicy baz danych - RestaurantAddresses.
        /// </summary>
        /// <param name="id">Klucz podstawowy adresu restauracji</param>
        /// <returns>Operację asynchroniczną, która zwraca listę obiektów typu RestaurantAddress</returns>
        public async Task<RestaurantAddress> FindById(long id)
        {
            return await _context.RestaurantAddresses
                .FirstOrDefaultAsync(address => address.Id == id);
        }

        /// <summary>
        /// Metoda Update przyjmuje obiekt typu MenuItem, następnie według argumentu, aktualizuje rekord z tablicy baz danych - RestaurantAddress.
        /// </summary>
        /// <param name="address">Obiekt typu RestaurantAddress</param>
        /// <returns>Operację asynchroniczną, która zwraca obiekt typu RestaurantAddress</returns>
        public async Task<RestaurantAddress> Update(RestaurantAddress address)
        {
            _context.Entry(address).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return address;
        }
    }
}
