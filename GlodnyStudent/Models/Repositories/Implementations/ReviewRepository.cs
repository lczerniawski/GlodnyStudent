using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlodnyStudent.Data;
using GlodnyStudent.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GlodnyStudent.Models.Repositories.Implementations
{
    /// <summary>
    /// Reprezentuje repozytorium opinii.
    /// </summary>
    public class ReviewRepository : IReviewRepository
    {
        /// <summary>
        /// Połączenia do bazy danych. 
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Konstruktor ReviewRepository przyjmuje jako argument połączenia do bazy danych.
        /// </summary>
        /// <param name="context">Podłączenia do bazy danych</param>
        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Metoda Create przyjmuje obiekt typu Review, następnie według argumentu, tworzy nowy rekord w tablicę baz danych - Reviews.
        /// </summary>
        /// <param name="review">Obiekt typu Review</param>
        /// <returns>Operację asynchroniczną, która zwraca obiekt typu Review</returns>
        public async Task<Review> Create(Review review)
        {
            Review result = _context.Reviews.Add(review).Entity;

            await _context.SaveChangesAsync();

            return result;
        }

        /// <summary>
        /// Metoda Delete przyjmuje id opinii i na jej podstawie asynchronicznie usuwa rekord z tablicy baz danych - Reviews.
        /// </summary>
        /// <param name="id">Klucz podstawowy opinii</param>
        /// <returns>Operację asynchroniczną</returns>
        public async Task Delete(long id)
        {
            Review result = await _context.Reviews
                    .FirstOrDefaultAsync(review => review.Id == id);

            _context.Entry(result).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Metoda FindAll asynchronicznie zwraca listę wszystkich rekordów z tablicy baz danych - Reviews.
        /// </summary>
        /// <returns>Operację asynchroniczną, która zwraca listę obiektów typu Review</returns>
        public async Task<IEnumerable<Review>> FindAll()
        {
            return await _context.Reviews.ToListAsync();
        }

        /// <summary>
        /// Metoda FindAllByRestaurantId przyjmuje id restauracji i na jej podstawie asynchronicznie zwraca 
        /// listę rekordów z tablicy baz danych - Reviews.
        /// </summary>
        /// <param name="restaurantId">Klucz obcy restauracji</param>
        /// <returns>Operację asynchroniczną, która zwraca listę obiektów typu Restaurant</returns>
        public async Task<IEnumerable<Review>> FindAllByRestaurantId(long restaurantId)
        {
            return await _context.Reviews
                    .Where(image => image.RestaurantId == restaurantId)
                    .ToListAsync();
        }

        /// <summary>
        /// Metoda FindById przyjmuje Id opinii, następnie według argumentu, szuka rekord z tablicy baz danych - Reviews.
        /// </summary>
        /// <param name="id">Klucz podstawowy opinii</param>
        /// <returns>Operację asynchroniczną, która zwraca obiekt typu Review</returns>
        public async Task<Review> FindById(long id)
        {
            return await _context.Reviews
                .FirstOrDefaultAsync(review => review.Id == id);
        }

        /// <summary>
        /// Metoda Update przyjmuje obiekt typu Review, następnie według argumentu, aktualizuje rekord z tablicy baz danych - Reviews.
        /// </summary>
        /// <param name="review">Obiekt typu Review</param>
        /// <returns>Operację asynchroniczną, która zwraca obiekt typu Reviews</returns>
        public async Task<Review> Update(Review review)
        {
            _context.Entry(review).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return review;
        }
    }
}
