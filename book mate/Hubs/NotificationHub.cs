using Microsoft.AspNetCore.SignalR;

namespace book_mate.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", user, message);
        }
        
    }
    
}
