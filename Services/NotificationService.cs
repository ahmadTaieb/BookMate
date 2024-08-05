
using book_mate.Hubs;
using BookMate.DataAccess.Data;
using BookMate.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<NotificationService> _logger;
        public NotificationService(ApplicationDbContext context, IHubContext<NotificationHub> hubContext,ILogger<NotificationService> logger)
        {
            _context = context;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task AddNotificationAsync(string userId, string message)
        {
            var notification = new Notification
            {
                ApplicationUserId = userId,
                Message = message,
                Date = DateTime.Now,
                IsRead = false
            };
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            //await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification",notification );
            _logger.LogInformation("Notification saved to database for user {UserId}", userId);

            //await _hubContext.Clients.All.SendAsync("ReceiveNotification", "message","user");
            _logger.LogInformation("Notification sent to all clients");
        }

        public async Task<List<Notification>> GetNotificationsAsync(string userId)
        {
            return _context.Notifications.Where(n => n.ApplicationUserId == userId).ToList();
        }
        public async Task<List<Notification>> GetAllAsync()
        {
            return _context.Notifications.ToList();
        }
        public async Task CreateNotificationForReportAsync(Report report)
        {
            var message = $"New report created: {report}";
            await AddNotificationAsync(report.ApplicationUserId, message);
        }
    }
}
