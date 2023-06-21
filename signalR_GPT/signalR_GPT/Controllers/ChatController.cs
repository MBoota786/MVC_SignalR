using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using signalR_GPT.Data;
using signalR_GPT.Hubs;
using signalR_GPT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace signalR_GPT.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ApplicationDbContext _dbContext;

        public ChatController(UserManager<User> userManager, IHubContext<ChatHub> hubContext, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _hubContext = hubContext;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var users = await _userManager.Users.Where(u => u.Id != currentUser.Id).ToListAsync();

            return View(users);
        }

        //public async Task<IActionResult> Conversation(string receiverId)
        //{
        //    var currentUser = await _userManager.GetUserAsync(User);
        //    var receiver = await _userManager.FindByIdAsync(receiverId);

        //    if (receiver == null)
        //    {
        //        return NotFound();
        //    }

        //    var messages = await _dbContext.Messages
        //        .Where(m => (m.SenderId == currentUser.Id && m.ReceiverId == receiver.Id)
        //                    || (m.SenderId == receiver.Id && m.ReceiverId == currentUser.Id))
        //        .OrderBy(m => m.SentTime)
        //        .ToListAsync();

        //    foreach (var message in messages)
        //    {
        //        if (message.SenderId == currentUser.Id)
        //        {
        //            message.IsSender = true;
        //            message.IsReceiver = false;
        //        }
        //        else
        //        {
        //            message.IsSender = false;
        //            message.IsReceiver = true;
        //        }
        //    }

        //    ViewBag.ReceiverId = receiverId;
        //    ViewBag.ReceiverName = receiver.Email;

        //    return View(messages);
        //}
        public async Task<IActionResult> Conversation(string receiverId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var receiver = await _userManager.FindByIdAsync(receiverId);

            if (receiver == null)
            {
                return NotFound();
            }

            var messages = await _dbContext.Messages
                .Where(m => (m.SenderId == currentUser.Id && m.ReceiverId == receiver.Id)
                            || (m.SenderId == receiver.Id && m.ReceiverId == currentUser.Id))
                .OrderBy(m => m.SentTime)
                .ToListAsync();

            foreach (var message in messages)
            {
                if (message.SenderId == currentUser.Id)
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

            return View(messages);
        }


    }
}
