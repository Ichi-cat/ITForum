using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ITForum.Api.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public async Task Send(string userId, string message)
        {
            //await Clients.All.SendAsync("Receive", message);
            // получение текущего пользователя, который отправил сообщение
            var userName = Context.UserIdentifier;
            if (userName == null) return;
            await Clients.Users(userName, userId).SendAsync("Receive", message, userName);
        }
    }
}
