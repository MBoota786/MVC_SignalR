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

            ViewBag.users = user;

            var model = new GroupChatViewModel
            {
                User = _dbContext.Users.ToList(),
                Group = group,
                Messages = messages
            };

            return View(model);
        }

        [HttpGet]
        public string GetUserName(string userId)
        {
            // Fetch the username based on the userId from your data source
            var userName = _dbContext.Users
                .Where(u => u.Id == userId)
                .Select(u => u.UserName)
                .FirstOrDefault();

            return userName;
        }


    }
}
