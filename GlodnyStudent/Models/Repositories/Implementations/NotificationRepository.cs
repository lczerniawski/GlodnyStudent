using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlodnyStudent.Data;
using GlodnyStudent.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GlodnyStudent.Models.Repositories.Implementations
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Notification> Create(Notification notification)
        {
            Notification result = _context.Notifications.Add(notification).Entity;

            await _context.SaveChangesAsync();

            return result;
        }

        public async Task Delete(long id)
        {
            Notification result = await _context.Notifications
                .FirstOrDefaultAsync(review => review.Id == id);

            _context.Entry(result).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Notification>> FindAll()
        {
            return await _context.Notifications.ToListAsync();
        }

        public async Task<Notification> FindById(long id)
        {
            return await _context.Notifications
                .FirstOrDefaultAsync(review => review.Id == id);
        }

        public async Task<Notification> Update(Notification notification)
        {
            _context.Entry(notification).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return notification;
        }
    }
}
