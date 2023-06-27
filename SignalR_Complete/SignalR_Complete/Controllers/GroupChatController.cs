using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalR_Complete.Data;
using SignalR_Complete.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Complete.Controllers
{
    public class GroupChatController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public GroupChatController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        
        public IActionResult Index(int groupId)
        {
            var currentUser= User.Identity.Name;
            
            var user = _dbContext.Users.Where(x => x.UserName == currentUser).FirstOrDefault();
            ViewBag.senderId = user.Id;

            
            var group = _dbContext.Group.Include(g => g.GroupUsers).FirstOrDefault(g => g.Id == groupId);
            var group1 = _dbContext.Group.Include(g => g.GroupUsers).FirstOrDefault(g => g.Id == groupId);

            if (group == null)
            {
                return NotFound();
            }

            if (!group.GroupUsers.Any(gu => gu.UserId == user.Id))
            {
                return Forbid();
            }

            var messages = _dbContext.GroupMessage
                .Where(gm => gm.GroupId == groupId)
                .OrderBy(gm => gm.Timestamp)
                .ToList();

            var model = new GroupChatViewModel
            {
                User = user,
                Group = group,
                Messages = messages
            };

            return View(model);
        }

        //[HttpPost]
        //public IActionResult SendGroupMessage(int groupId, string message)
        //{
        //    var userId = User.Identity.Name;
        //    var group = _dbContext.Group.FirstOrDefault(g => g.Id == groupId);

        //    if (group == null)
        //    {
        //        return NotFound();
        //    }

        //    if (!_dbContext.GroupUser.Any(gu => gu.GroupId == groupId && gu.UserId == userId))
        //    {
        //        return Forbid();
        //    }

        //    var groupMessage = new GroupMessage
        //    {
        //        GroupId = groupId,
        //        SenderId = userId,
        //        Message = message,
        //        Timestamp = DateTime.Now
        //    };

        //    _dbContext.GroupMessage.Add(groupMessage);
        //    _dbContext.SaveChanges();

        //    var connectionIds = _dbContext.GroupUser
        //        .Where(gu => gu.GroupId == groupId && gu.UserId != userId)
        //        .Select(gu => gu.User.ConnectionId)
        //        .ToList();

        //    var isRead = false;
        //    if (connectionIds != null && connectionIds.Count > 0)
        //    {
        //        isRead = true;
        //    }

        //    foreach (var connectionId in connectionIds)
        //    {
        //        Clients.Client(connectionId).SendAsync("ReceiveGroupMessage", groupId, userId, message, groupMessage.Timestamp, isRead);
        //    }

        //    return Ok();
        //}

        //[HttpPost]
        //public IActionResult MarkGroupMessageAsRead(int groupId)
        //{
        //    var userId = User.Identity.Name;

        //    var groupUsers = _dbContext.GroupUser
        //        .Where(gu => gu.GroupId == groupId && gu.UserId == userId)
        //        .ToList();

        //    if (groupUsers.Count > 0)
        //    {
        //        var groupMessageIds = _dbContext.GroupMessage
        //            .Where(gm => gm.GroupId == groupId && !gm.IsReaded)
        //            .Select(gm => gm.Id)
        //            .ToList();

        //        var groupMessages = _dbContext.GroupMessage
        //            .Where(gm => groupMessageIds.Contains(gm.Id))
        //            .ToList();

        //        foreach (var message in groupMessages)
        //        {
        //            message.IsReaded = true;
        //        }

        //        _dbContext.SaveChanges();

        //        var connectionIds = _dbContext.GroupUser
        //            .Where(gu => gu.GroupId == groupId && gu.UserId != userId)
        //            .Select(gu => gu.User.ConnectionId)
        //            .ToList();

        //        foreach (var connectionId in connectionIds)
        //        {
        //            Clients.Client(connectionId).SendAsync("MarkAsRead");
        //        }
        //    }

        //    return Ok();
        //}
    }
}
