using System.Collections.Generic;
using System.Threading.Tasks;
using GlodnyStudent.Data;
using GlodnyStudent.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GlodnyStudent.Models.Repositories.Implementations
{
    /// <summary>
    /// Reprezentuje repozytorium powiadomień.
    /// </summary>
    public class NotificationRepository : INotificationRepository
    {
        /// <summary>
        /// Połączenia do bazy danych. 
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Konstruktor NotificationRepository przyjmuje jako argument połączenia do bazy danych.
        /// </summary>
        /// <param name="context">Podłączenia do bazy danych</param>
        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Metoda Create przyjmuje obiekt typu Notification, następnie według argumentu, tworzy nowy rekord w tablicę baz danych - Notifications.
        /// </summary>
        /// <param name="notification">Obiekt typu Notification</param>
        /// <returns>Operację asynchroniczną, która zwraca obiekt typu Notification</returns>
        public async Task<Notification> Create(Notification notification)
        {
            Notification result = _context.Notifications.Add(notification).Entity;

            await _context.SaveChangesAsync();

            return result;
        }

        /// <summary>
        /// Metoda Delete przyjmuje id powiadomienia i na jej podstawie asynchronicznie usuwa rekord z tablicy baz danych - Notifications.
        /// </summary>
        /// <param name="id">Klucz podstawowy powiadomienia</param>
        /// <returns>Operację asynchroniczną</returns>
        public async Task Delete(long id)
        {
            Notification result = await _context.Notifications
                .FirstOrDefaultAsync(review => review.Id == id);

            _context.Entry(result).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Metoda FindAll asynchronicznie zwraca listę wszystkich rekordów z tablicy baz danych - Notifications.
        /// </summary>
        /// <returns>Operację asynchroniczną, która zwraca listę obiektów typu Notification</returns>
        public async Task<IEnumerable<Notification>> FindAll()
        {
            return await _context.Notifications.ToListAsync();
        }

        /// <summary>
        /// Metoda FindById przyjmuje Id powiadomienia, następnie według argumentu, szuka rekord z tablicy baz danych - Notifications.
        /// </summary>
        /// <param name="id">Klucz podstawowy powiadomienia</param>
        /// <returns>Operację asynchroniczną, która zwraca obiekt typu Notification</returns>
        public async Task<Notification> FindById(long id)
        {
            return await _context.Notifications
                .FirstOrDefaultAsync(review => review.Id == id);
        }

        /// <summary>
        /// Metoda Update przyjmuje obiekt typu Notification, następnie według argumentu, aktualizuje rekord z tablicy baz danych - Notifications.
        /// </summary>
        /// <param name="notification">Obiekt typu Notification</param>
        /// <returns>Operację asynchroniczną, która zwraca obiekt typu Notification</returns>
        public async Task<Notification> Update(Notification notification)
        {
            _context.Entry(notification).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return notification;
        }
    }
}
