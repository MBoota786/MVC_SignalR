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
        public IActionResult Index(string participantId)
        {
            var userName = User.Identity.Name;
            var currentUsers = _dbContext.Users.Where(x=>x.UserName == userName).FirstOrDefault();
            var participant = _dbContext.Users.FirstOrDefault(u => u.Id == participantId);

            if (participant == null)
            {
                return NotFound();
            }

            var messages = _dbContext.PrivateMessage
                            .Include(x => x.Sender)
                            .Where(pm => (pm.SenderId == currentUsers.Id && pm.ReceiverId == participantId) ||
                                            (pm.SenderId == participantId && pm.ReceiverId == currentUsers.Id))
                            .OrderBy(pm => pm.Timestamp)
                            .ToList();

            var model = new ChatViewModel
            {
                User = _dbContext.Users.FirstOrDefault(u => u.Id == currentUsers.Id),
                Participant = participant,
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
