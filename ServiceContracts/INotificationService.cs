using BookMate.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface INotificationService
    {
        Task AddNotificationAsync(string userId, string message);
        Task<List<Notification>> GetNotificationsAsync(string userId);
        Task CreateNotificationForReportAsync(Report report);
        Task<List<Notification>> GetAllAsync();
    }
}
