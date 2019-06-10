using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlodnyStudent.Data;
using GlodnyStudent.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GlodnyStudent.Models.Repositories.Implementations
{
    /// <summary>
    /// Reprezentuje repozytorium obrazów.
    /// </summary>
    public class ImageRepository : IImageRepository
    {
        /// <summary>
        /// Połączenia do bazy danych. 
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Konstruktor ImageRepository przyjmuje jako argument połączenia do bazy danych.
        /// </summary>
        /// <param name="context">Podłączenia do bazy danych</param>
        public ImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Metoda Create przyjmuje obiekt typu Image, według argumentu tworzy nowy rekord w tablicę baz danych - Images 
        /// i asynchronicznie zwraca zapisany obiekt.
        /// </summary>
        /// <param name="image">Obiekt typu Image</param>
        /// <returns>Operację asynchroniczną, która zwraca obiekt typu Image</returns>
        public async Task<Image> Create(Image image)
        {
            Image result = _context.Images.Add(image).Entity;

            await _context.SaveChangesAsync();

            return result;
        }

        /// <summary>
        /// Metoda Delete przyjmuje id obrazu i na jej podstawie asynchronicznie usuwa rekord z tablicy baz danych - Images.
        /// </summary>
        /// <param name="id">Id obrazu</param>
        /// <returns>Operację asynchroniczną</returns>
        public async Task Delete(long id)
        {
            Image result = await _context.Images.FirstOrDefaultAsync(image => image.Id == id);

            _context.Entry(result).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Metoda FindAll asynchronicznie zwraca listę wszystkich rekordów z tablicy baz danych - Images.
        /// </summary>
        /// <returns>Operację asynchroniczną, która zwraca listę obiektów typu Image</returns>
        public async Task<IEnumerable<Image>> FindAll()
        {
            return await _context.Images.ToListAsync();
        }

        /// <summary>
        /// Metoda FindAllByRestaurantId przyjmuje id restauracji i na jej podstawie asynchronicznie zwraca 
        /// listę rekordów z tablicy baz danych - Images.
        /// </summary>
        /// <param name="restaurantId">Id restauracji</param>
        /// <returns>Operację asynchroniczną, która zwraca listę obiektów typu Image</returns>
        public async Task<IEnumerable<Image>> FindAllByRestaurantId(long restaurantId)
        {
            return await _context.Images
                    .Where(image => image.RestaurantId == restaurantId)
                    .ToListAsync();
        }

        /// <summary>
        /// Metoda FindById przyjmuje ID obrazu, według argumentu szuka rekord z tablicy baz danych - Images 
        /// i asynchronicznie zwraca dany obiekt.
        /// </summary>
        /// <param name="id">ID obrazu</param>
        /// <returns>Operację asynchroniczną, która zwraca obiekt typu Image</returns>
        public async Task<Image> FindById(long id)
        {
            return await _context.Images.FirstOrDefaultAsync(image => image.Id == id);
        }

        /// <summary>
        /// Metoda Update przyjmuje obiekt typu Image, według argumentu aktualizuje rekord z tablicy baz danych - Images 
        /// i asynchronicznie zwraca dany obiekt.
        /// </summary>
        /// <param name="image">Obiekt typu Image</param>
        /// <returns>Operację asynchroniczną, która zwraca obiekt typu Image</returns>
        public async Task<Image> Update(Image image)
        {
            _context.Entry(image).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return image;
        }
    }
}
