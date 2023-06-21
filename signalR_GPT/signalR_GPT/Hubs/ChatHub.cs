using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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

        //____________________________ 1st  simple send message ____________________________
        #region Simple_Send_Message
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
                IsSender = true,
                IsRead = false,
                IsReceiver = false
            };

            await _dbContext.Messages.AddAsync(message);
            await _dbContext.SaveChangesAsync();

            await Clients.User(receiverId).SendAsync("ReceiveMessage", Context.User.Identity.Name, content, false);
            await Clients.Caller.SendAsync("ReceiveMessage", Context.User.Identity.Name, content, true);
        }

        #endregion

        //____________________________ 2nd  only update Message for login Users ____________________________
        #region readedMessage_for_LogedINUser
        //private static readonly Dictionary<string, List<string>> UserConnections = new Dictionary<string, List<string>>();

        //private readonly ApplicationDbContext _dbContext;

        //public ChatHub(ApplicationDbContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        //public override async Task OnConnectedAsync()
        //{
        //    var userId = Context.UserIdentifier;
        //    var connectionId = Context.ConnectionId;

        //    if (!UserConnections.ContainsKey(userId))
        //        UserConnections[userId] = new List<string>();

        //    UserConnections[userId].Add(connectionId);

        //    await base.OnConnectedAsync();
        //}

        //public override async Task OnDisconnectedAsync(Exception exception)
        //{
        //    var userId = Context.UserIdentifier;
        //    var connectionId = Context.ConnectionId;

        //    if (UserConnections.ContainsKey(userId))
        //    {
        //        UserConnections[userId].Remove(connectionId);
        //        if (UserConnections[userId].Count == 0)
        //            UserConnections.Remove(userId);
        //    }

        //    await base.OnDisconnectedAsync(exception);
        //}

        //public async Task SendMessage(string receiverId, string content)
        //{
        //    var senderId = Context.UserIdentifier;
        //    var message = new Message
        //    {
        //        SenderId = senderId,
        //        ReceiverId = receiverId,
        //        Content = content,
        //        SentTime = DateTime.Now,
        //        IsSender = true,
        //        IsRead = false,
        //        IsReceiver = false
        //    };

        //    await _dbContext.Messages.AddAsync(message);
        //    await _dbContext.SaveChangesAsync();

        //    if (UserConnections.ContainsKey(receiverId))
        //    {
        //        message.IsRead = true;
        //        message.IsReceiver = true;
        //        await _dbContext.SaveChangesAsync();
        //    }

        //    await Clients.User(receiverId).SendAsync("ReceiveMessage", Context.User.Identity.Name, content, message.IsRead);
        //    await Clients.Caller.SendAsync("ReceiveMessage", Context.User.Identity.Name, content, true);
        //}
        #endregion


    }
}
