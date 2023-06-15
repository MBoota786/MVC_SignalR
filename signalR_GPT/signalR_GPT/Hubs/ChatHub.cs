using Microsoft.AspNetCore.SignalR;
using signalR_GPT.Data;
using signalR_GPT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace signalR_GPT.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserConnectionManager _userConnectionManager;

        public ChatHub(ApplicationDbContext dbContext, UserConnectionManager userConnectionManager)
        {
            _dbContext = dbContext;
            _userConnectionManager = userConnectionManager;
        }

        public async Task SendMessage(string receiverId, string content)
        {
            var senderId = Context.UserIdentifier;
            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                SentTime = DateTime.Now,
                IsRead = false,
                IsSender = true,
                IsReceiver = false
            };

            await _dbContext.Messages.AddAsync(message);
            await _dbContext.SaveChangesAsync();

            var receiverMessage = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                SentTime = DateTime.Now,
                IsRead = false,
                IsSender = false,
                IsReceiver = true
            };

            await _dbContext.Messages.AddAsync(receiverMessage);
            await _dbContext.SaveChangesAsync();

            await Clients.User(receiverId).SendAsync("ReceiveMessage", Context.User.Identity.Name, content, false);
            await Clients.Caller.SendAsync("ReceiveMessage", Context.User.Identity.Name, content, true);
        }
    }
}
