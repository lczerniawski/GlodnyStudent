using System.Threading.Tasks;
using GlodnyStudent.Data;
using GlodnyStudent.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GlodnyStudent.Models.Repositories.Implementations
{
    /// <summary>
    /// Reprezentuje repozytorium rodzaju kuchni.
    /// </summary>
    public class CuisineRepository : ICuisineRepository
    {
        /// <summary>
        /// Połączenia do bazy danych. 
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Konstruktor CuisineRepository przyjmuje jako argument połączenia do bazy danych.
        /// </summary>
        /// <param name="context">Podłączenia do bazy danych</param>
        public CuisineRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Metoda Create przyjmuje obiekt typu Cuisine, następnie według argumentu, tworzy nowy rekord w tablicę baz danych - Cuisines.
        /// </summary>
        /// <param name="cuisine">Obiekt typu Cuisine</param>
        /// <returns>Operację asynchroniczną, która zwraca obiekt typu Cuisine</returns>
        public async Task<Cuisine> Create(Cuisine cuisine)
        {
            Cuisine result = _context.Cuisines.Add(cuisine).Entity;

            await _context.SaveChangesAsync();

            return result;
        }

<<<<<<< HEAD
        public async Task Delete(long id)
=======
        /// <summary>
        /// Metoda Delete przyjmuje nazwę typu kuchni i na jej podstawie asynchronicznie usuwa rekord z tablicy baz danych - Cuisines.
        /// </summary>
        /// <param name="name">Nazwa typu kuchni</param>
        /// <returns>Operację asynchroniczną</returns>
        public async Task Delete(string name)
>>>>>>> de5da774fa2009f59eb6608ee6532041efe99345
        {
            Cuisine result = await _context.Cuisines.FirstOrDefaultAsync(cuisine => cuisine.Id == id);

            _context.Entry(result).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Metoda FindAll zwraca tablicę wszystkich rekordów z tablicy baz danych - Cuisines.
        /// </summary>
        /// <returns>Operację asynchroniczną, która zwraca tablice obiektów typu Cuisine</returns>
        public async Task<Cuisine[]> FindAll()
        {
            return await _context.Cuisines.ToArrayAsync();
        }

<<<<<<< HEAD
        public async Task<Cuisine> FindById(long id)
=======
        /// <summary>
        /// Metoda FindByName przyjmuje nazwę typu kuchni, następnie według argumentu, szuka rekord z tablicy baz danych - Cuisines.
        /// </summary>
        /// <param name="name">Nazwa typu kuchni</param>
        /// <returns>Operację asynchroniczną, która zwraca obiekt typu Cuisine</returns>
        public async Task<Cuisine> FindByName(string name)
>>>>>>> de5da774fa2009f59eb6608ee6532041efe99345
        {
            return await _context.Cuisines.FirstOrDefaultAsync(cuisine => cuisine.Id == id);
        }

        /// <summary>
        /// Metoda Update przyjmuje obiekt typu Cuisine, następnie według argumentu, aktualizuje rekord z tablicy baz danych - Cuisines.
        /// </summary>
        /// <param name="cuisine">Obiekt typu Cuisine</param>
        /// <returns>Operację asynchroniczną, która zwraca obiekt typu Cuisine</returns>
        public async Task<Cuisine> Update(Cuisine cuisine)
        {
            _context.Entry(cuisine).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return cuisine;
        }
    }
}
