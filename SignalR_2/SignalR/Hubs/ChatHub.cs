using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        /*
         From the above sample code, in the OnConnectedAsyc method,
        we could get the ConnectId and the User Name, then, you could store them in the database. Then,
        you can add the SendMessageToUser method in the ChatHub.cs. In this method, 
        you could query the database and find the connectionId based on the receiver name,
        after that using Clients.Client("connectionId").SendAsync() method to send the message to a specific user.
         */
        public override Task OnConnectedAsync()
        {
            Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            return base.OnConnectedAsync();
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public Task SendMessageToGroup(string sender, string receiver, string message)
        {
            return Clients.Group(receiver).SendAsync("ReceiveMessage", sender, message);
        }
    }
}
