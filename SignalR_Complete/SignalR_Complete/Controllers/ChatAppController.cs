using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalR_Complete.Data;
using SignalR_Complete.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalR_Complete.Controllers
{
    public class ChatAppController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ChatAppController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        [Authorize]
        public IActionResult Index(string receiverId)
        {
            var userName = User.Identity.Name;
            var currentUsers = _dbContext.Users.Where(x=>x.UserName == userName).FirstOrDefault();
            var receiver = _dbContext.Users.FirstOrDefault(u => u.Id == receiverId);

            if (receiver == null)
            {
                return NotFound();
            }

            var messages = _dbContext.PrivateMessage
                            .Include(x => x.Sender)
                            .Where(pm => (pm.SenderId == currentUsers.Id && pm.ReceiverId == receiverId) ||
                                            (pm.SenderId == receiverId && pm.ReceiverId == currentUsers.Id))
                            .OrderBy(pm => pm.Timestamp)
                            .ToList();

            foreach (var message in messages)
            {
                if (message.SenderId == currentUsers.Id)
                {
                    message.IsSender = true;
                    message.IsReceiver = false;
                }
                else
                {
                    message.IsSender = false;
                    message.IsReceiver = true;
                }
            }

            ViewBag.ReceiverId = receiverId;
            ViewBag.ReceiverName = receiver.Email;

            var model = new ChatViewModel
            {
                User = _dbContext.Users.FirstOrDefault(u => u.Id == currentUsers.Id),
                Recever = receiver,
                Messages = messages
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult MarkPrivateMessageAsRead(string senderId)
        {
            var userId = User.Identity.Name;
            var messages = _dbContext.PrivateMessage
                .Where(pm => pm.SenderId == senderId && pm.ReceiverId == userId && !pm.IsReaded)
                .ToList();

            foreach (var message in messages)
            {
                message.IsReaded = true;
            }

            _dbContext.SaveChanges();

            return Ok();
        }
    }
}
