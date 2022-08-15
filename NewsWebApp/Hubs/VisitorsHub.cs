using Microsoft.AspNetCore.SignalR;

using System.Threading.Tasks;

namespace NewsWebApp.Hubs
{
    public class VisitorsHub : Hub
    {
       
       
            public async Task SendMessage(string user, string message)
            {
                await Clients.All.SendAsync("ReceiveMessage", user, message);
            }
       
    }
}


