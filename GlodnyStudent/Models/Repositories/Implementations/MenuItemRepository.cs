using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlodnyStudent.Data;
using GlodnyStudent.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GlodnyStudent.Models.Repositories.Implementations
{
    /// <summary>
    /// Reprezentuje repozytorium elementu menu.
    /// </summary>
    public class MenuItemRepository : IMenuItemRepository
    {
        /// <summary>
        /// Połączenia do bazy danych. 
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Konstruktor MenuItemRepository przyjmuje jako argument połączenia do bazy danych.
        /// </summary>
        /// <param name="context">Podłączenia do bazy danych</param>
        public MenuItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Metoda Create przyjmuje obiekt typu MenuItem, następnie według argumentu, tworzy nowy rekord w tablicę baz danych - MenuItems.
        /// </summary>
        /// <param name="menuItem">Obiekt typu MenuItem</param>
        /// <returns>Operację asynchroniczną, która zwraca obiekt typu MenuItem</returns>
        public async Task<MenuItem> Create(MenuItem menuItem)
        {
            MenuItem result = _context.MenuItems.Add(menuItem).Entity;

            await _context.SaveChangesAsync();

            return result;
        }

        /// <summary>
        /// Metoda Delete przyjmuje id elementu menu i na jej podstawie asynchronicznie usuwa rekord z tablicy baz danych - MenuItems.
        /// </summary>
        /// <param name="id">Klucz podstawowy elementu menu</param>
        /// <returns>Operację asynchroniczną</returns>
        public async Task Delete(long id)
        {
            MenuItem result = await _context.MenuItems.FirstOrDefaultAsync(item => item.Id == id);

            _context.Entry(result).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Metoda FindAll asynchronicznie zwraca listę wszystkich rekordów z tablicy baz danych - MenuItems.
        /// </summary>
        /// <returns>Operację asynchroniczną, która zwraca listę obiektów typu MenuItem</returns>
        public async Task<IEnumerable<MenuItem>> FindAll()
        {
            return await _context.MenuItems.ToListAsync();
        }

        /// <summary>
        /// Metoda FindAllByRestaurantId przyjmuje id restauracji i na jej podstawie asynchronicznie zwraca 
        /// listę rekordów z tablicy baz danych - MenuItems.
        /// </summary>
        /// <param name="restaurantId">Klucz obcy restauracji</param>
        /// <returns>Operację asynchroniczną, która zwraca listę obiektów typu MenuItem</returns>
        public async Task<IEnumerable<MenuItem>> FindAllByRestaurantId(long restaurantId)
        {
            return await _context.MenuItems
                    .Where(item => item.RestaurantId == restaurantId)
                    .ToListAsync();
        }

        /// <summary>
        /// Metoda FindById przyjmuje Id elementu menu, następnie według argumentu, szuka rekord z tablicy baz danych - MenuItems.
        /// </summary>
        /// <param name="id">Klucz podstawowy elementu menu</param>
        /// <returns>Operację asynchroniczną, która zwraca obiekt typu MenuItem</returns>
        public async Task<MenuItem> FindById(long id)
        {
            return await _context.MenuItems.FirstOrDefaultAsync(item => item.Id == id);
        }

        /// <summary>
        /// Metoda Update przyjmuje obiekt typu MenuItem, następnie według argumentu, aktualizuje rekord z tablicy baz danych - MenuItems.
        /// </summary>
        /// <param name="menuItem">Obiekt typu MenuItem</param>
        /// <returns>Operację asynchroniczną, która zwraca obiekt typu MenuItem</returns>
        public async Task<MenuItem> Update(MenuItem menuItem)
        {
            _context.Entry(menuItem).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return menuItem;
        }
    }
}
