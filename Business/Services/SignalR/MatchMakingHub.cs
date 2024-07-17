using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Business.Services.SignalR
{
    [Authorize]
    public class MatchMakingHub : Hub
    {
        public async Task SendMessagesAsync(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
