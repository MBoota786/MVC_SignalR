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
    public class GroupAssignmentController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public GroupAssignmentController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        
        public IActionResult Index()
        {
            var model = new GroupAssignmentViewModel
            {
                Users = _dbContext.Users.ToList(),
                Groups = _dbContext.Group.ToList()
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult GetGroupUsers(int groupId)
        {
            var groupUsers = _dbContext.GroupUser
                .Where(gu => gu.GroupId == groupId)
                .Include(gu => gu.User)
                .Select(gu => gu.User)
                .ToList();

            var users = groupUsers.Select(u => new { Id = u.Id, UserName = u.UserName }).ToList();

            return Json(new { users });
        }

        [HttpPost]
        public IActionResult AssignUsers(int groupId, string[] userIds)
        {
            var existingGroupUsers = _dbContext.GroupUser.Where(gu => gu.GroupId == groupId).ToList();

            // Remove users who are no longer assigned to the group
            var removedUsers = existingGroupUsers.Where(gu => !userIds.Contains(gu.UserId)).ToList();
            _dbContext.GroupUser.RemoveRange(removedUsers);

            // Add users who are newly assigned to the group
            var newUsers = userIds.Where(userId => existingGroupUsers.All(gu => gu.UserId != userId)).Select(userId => new GroupUser
            {
                GroupId = groupId,
                UserId = userId
            }).ToList();
            _dbContext.GroupUser.AddRange(newUsers);

            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
