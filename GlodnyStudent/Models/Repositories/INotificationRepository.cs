using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlodnyStudent.Models.Domain;

namespace GlodnyStudent.Models.Repositories
{
    public interface INotificationRepository : ICrudRepository<Notification>
    {
    }
}
