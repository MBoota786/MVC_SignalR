using Microsoft.AspNetCore.SignalR;
using SignalR_Complete.Data;
using SignalR_Complete.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Complete.Hubs
{
    public class ChatHub:Hub
    {
        private readonly ApplicationDbContext _dbContext;

        public ChatHub(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task OnConnectedAsync()
        {
            var userName = Context.User.Identity.Name;
            var user = _dbContext.Users.FirstOrDefault(u => u.UserName == userName);

            if (user != null)
            {
                user.ConnectionId = Context.ConnectionId;
                await _dbContext.SaveChangesAsync();
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userName = Context.User.Identity.Name;
            var user = _dbContext.Users.FirstOrDefault(u => u.UserName == userName);

            if (user != null)
            {
                user.ConnectionId = null;
                await _dbContext.SaveChangesAsync();
            }

            await base.OnDisconnectedAsync(exception);
        }

        //_______________ Private _________
        public async Task SendMessage(string userId, string message)
        {
            // Save the private chat message in the database
            //var senderId = Context.UserIdentifier;
            //var privateMessage = new PrivateMessage
            //{
            //    SenderId = senderId,
            //    ReceiverId = userId,
            //    Message = message,
            //    Timestamp = DateTime.Now
            //};

            //_dbContext.PrivateChat.Add(privateMessage);
            //await _dbContext.SaveChangesAsync();


            //await Clients.User(userId).SendAsync("ReceiveMessage", Context.User.Identity.Name, message, false);
            //await Clients.Caller.SendAsync("ReceiveMessage", Context.User.Identity.Name, message, true);


            var senderId = Context.UserIdentifier;
            var messages = new PrivateMessage
            {
                SenderId = senderId,
                ReceiverId = userId,
                Message = message,
                Timestamp = DateTime.Now,
                IsSender = true,
                IsReaded = false,
                IsReceiver = false
            };
            await _dbContext.PrivateMessage.AddAsync(messages);
            await _dbContext.SaveChangesAsync();

            var messageId = messages.Id;
            var timeStamp = messages.Timestamp.ToString("M/d/yyyy h:m:s tt");
            var receverId = messages.ReceiverId;

            await Clients.User(userId).SendAsync("ReceiveMessage", Context.User.Identity.Name , receverId, messageId, message, timeStamp, false);
            await Clients.Caller.SendAsync("ReceiveMessage", Context.User.Identity.Name, receverId, messageId, message, timeStamp, true);
        }
        public async Task DeletePrivateChatMessage(int messageId)
        {
            //go Login ha  --> Wohi  user Apnaa Messages Delete kr skaa ha
            var userId = Context.UserIdentifier;
            var message = _dbContext.PrivateChat.FirstOrDefault(pm => pm.Id == messageId && pm.SenderId == userId);

            if (message != null)
            {
                _dbContext.PrivateChat.Remove(message);
                await _dbContext.SaveChangesAsync();
                await Clients.All.SendAsync("MessageDeleted" , userId, messageId);
            }
        }


        //_______________ Group _________
        public async Task SendGroupMessage(int groupId, string message)
        {
            try
            {
                // Save the group chat message in the database
                var senderId = Context.UserIdentifier;
                //var senderId = Context.User.Identity.Name;
                var groupMessage = new GroupMessage
                {
                    GroupId = groupId,
                    SenderId = senderId,
                    Message = message,
                    Timestamp = DateTime.Now
                };

                _dbContext.GroupMessage.Add(groupMessage);
                await _dbContext.SaveChangesAsync();

                // Get the group members
                var groupMembers = _dbContext.GroupUser
                    .Where(gu => gu.GroupId == groupId && gu.UserId != senderId)
                    .Select(gu => gu.UserId)
                    .ToList();

                var groupMessageId = groupMessage.Id;

                if (groupMembers.Count > 0)
                {
                    // Send the message to all group members
                    foreach (var memberId in groupMembers)
                    {
                        await Clients.Users(memberId).SendAsync("ReceiveGroupMessage", groupId, senderId, groupMessageId, message, groupMessage.Timestamp,false);
                    }
                }
                await Clients.Caller.SendAsync("ReceiveGroupMessage", groupId, senderId, groupMessageId, message,groupMessage.Id, groupMessage.Timestamp, true);
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine(ex.ToString());
                throw; // Rethrow the exception if necessary
            }
            //// Save the group chat message in the database
            //var senderId = Context.User.Identity.Name;
            //var groupMessage = new GroupMessage
            //{
            //    GroupId = groupId,
            //    SenderId = senderId,
            //    Message = message,
            //    Timestamp = DateTime.Now
            //};

            //_dbContext.GroupMessage.Add(groupMessage);
            //await _dbContext.SaveChangesAsync();

            //// Get the connection IDs of the group members
            //List<string> connectionIds = GetGroupConnectionIds(groupId);

            //if (connectionIds != null && connectionIds.Count > 0)
            //{
            //    // Send the message to all group members
            //    await Clients.Clients(connectionIds).SendAsync("ReceiveGroupMessage", groupId, senderId, message, groupMessage.Timestamp, false);
            //}
        }
        public async Task DeleteGroupMessages(int groupId, int messageId )
        {
            var userId = Context.UserIdentifier;
            var message = _dbContext.GroupMessage.FirstOrDefault(gm => gm.Id == messageId && gm.GroupId == groupId && gm.SenderId == userId);
            if (message != null)
            {
                _dbContext.GroupMessage.Remove(message);
                await _dbContext.SaveChangesAsync();
                //await Clients.Group(groupId.ToString()).SendAsync("GroupMessageDeleted", groupId, messageId);
                await Clients.All.SendAsync("GroupMessageDeleted", groupId, messageId);
            }
        }



        public async Task SendImage(string userId, byte[] imageBytes)
        {
            //Save the image in the database
            var senderId = Context.User.Identity.Name;
            var image = new Image
            {
                SenderId = senderId,
                ReceiverId = userId,
                ImageBytes = imageBytes,
                Timestamp = DateTime.Now
            };

            _dbContext.Image.Add(image);
            await _dbContext.SaveChangesAsync();

            // Get the connection ID of the receiver
            //string receiverConnectionId = GetConnectionId(userId);

            //if (receiverConnectionId != null)
            //{
            //    // Send the image to the receiver
            //    await Clients.Client(receiverConnectionId).SendAsync("ReceiveImage", senderId, imageBytes, image.Timestamp, false);
            //}
        }

        public async Task AddToGroup(int groupId)
        {
            // Add the current user to the specified group
            var userId = Context.User.Identity.Name;
            var groupUser = new GroupUser
            {
                GroupId = groupId,
                UserId = userId
            };

            _dbContext.GroupUser.Add(groupUser);
            await _dbContext.SaveChangesAsync();

            // Get the connection ID of the current user
            string connectionId = Context.ConnectionId;

            // Add the connection ID to the group
            await Groups.AddToGroupAsync(connectionId, groupId.ToString());

            // Notify the client that they have been added to the group
            await Clients.Caller.SendAsync("AddedToGroup", groupId);
        }

        public async Task LeaveGroup(int groupId)
        {
            // Remove the current user from the specified group
            var userId = Context.User.Identity.Name;
            var groupUser = _dbContext.GroupUser.FirstOrDefault(gu => gu.GroupId == groupId && gu.UserId == userId);

            if (groupUser != null)
            {
                _dbContext.GroupUser.Remove(groupUser);
                await _dbContext.SaveChangesAsync();
            }

            // Get the connection ID of the current user
            string connectionId = Context.ConnectionId;

            // Remove the connection ID from the group
            await Groups.RemoveFromGroupAsync(connectionId, groupId.ToString());

            // Notify the client that they have left the group
            await Clients.Caller.SendAsync("LeftGroup", groupId);
        }

        public async Task MarkPrivateMessageAsRead(string userId)
        {
            // Mark the private messages between the current user and the specified user as read
            var senderId = Context.User.Identity.Name;
            var messages = _dbContext.PrivateMessage
                .Where(pm => pm.SenderId == senderId && pm.ReceiverId == userId && !pm.IsReaded)
                .ToList();

            foreach (var message in messages)
            {
                message.IsReaded = true;
            }

            await _dbContext.SaveChangesAsync();

            // Get the connection ID of the specified user
            //string userConnectionId = GetConnectionId(userId);

            //if (userConnectionId != null)
            //{
            //    // Send a signal to the specified user's client to update the read status of the messages
            //    await Clients.Client(userConnectionId).SendAsync("MarkAsRead");
            //}
        }

        public async Task MarkGroupMessageAsRead(int groupId)
        {
            // Mark the group messages in the specified group as read
            var userId = Context.User.Identity.Name;
            var groupUsers = _dbContext.GroupUser
                .Where(gu => gu.GroupId == groupId && gu.UserId == userId)
                .ToList();

            if (groupUsers.Count > 0)
            {
                var groupMessageIds = _dbContext.GroupMessage
                    .Where(gm => gm.GroupId == groupId && !gm.IsReaded)
                    .Select(gm => gm.Id)
                    .ToList();

                var groupMessages = _dbContext.GroupMessage
                    .Where(gm => groupMessageIds.Contains(gm.Id))
                    .ToList();

                foreach (var message in groupMessages)
                {
                    message.IsReaded = true;
                }

                await _dbContext.SaveChangesAsync();

                // Get the connection IDs of the group members
                List<string> connectionIds = GetGroupConnectionIds(groupId);

                if (connectionIds != null && connectionIds.Count > 0)
                {
                    // Send a signal to all group members' clients to update the read status of the messages
                    await Clients.Clients(connectionIds).SendAsync("MarkAsRead");
                }
            }
        }

        // Other methods and functionalities as per your requirements

        // Helper methods to retrieve connection IDs and group connection IDs

        //private string GetConnectionId(string userId)
        //{
        //    var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
        //    return user.ConnectionId;
        //}

        private List<string> GetGroupConnectionIds(int groupId)
        {
            var connectionIds = _dbContext.GroupUser
                .Where(gu => gu.GroupId == groupId)
                .Select(gu => gu.User.ConnectionId)
                .ToList();
            return connectionIds;
        }
    }
}
