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

        [HttpGet]
        public IActionResult AddGroup()
        {
            return View();
        }
        [HttpPost]
        public async  Task<IActionResult> AddGroup(Group model)
        {
            await _dbContext.Group.AddAsync(model);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Index(int groupId=0)
        {
            var model = new GroupAssignmentViewModel
            {
                Groups = _dbContext.Group.ToList(),
                Users = _dbContext.GroupUser     //user me     groupUsers   saa data dalan ga kyun kaa   ---> Ajax    GroupUser
                .Where(gu => gu.GroupId == 0)
                .Include(gu => gu.User)
                .Select(gu => gu.User)
                .ToList()
            };

            if (groupId > 0)
            {
                model = new GroupAssignmentViewModel
                {
                    Groups = _dbContext.Group.ToList(),
                    SelectedGroup = _dbContext.Group.FirstOrDefault(x=>x.Id == groupId).Name,
                    Users = _dbContext.GroupUser     //user me     groupUsers   saa data dalan ga kyun kaa   ---> Ajax    GroupUser
                    .Where(gu => gu.GroupId == groupId)
                    .Include(gu => gu.User)
                    .Select(gu => gu.User)
                    .ToList()
                };
            }
            else
            {
            
            }
            
            return View(model);
        }

        //[HttpPost]
        //public IActionResult Index(int groupId)
        //{
        //    var group = _dbContext.Group.FirstOrDefault(g => g.Id == groupId);
        //    if (group == null)
        //    {
        //        return RedirectToAction("GroupSelection");
        //    }

        //    var model = new GroupAssignmentViewModel
        //    {
        //        Groups = _dbContext.Group.ToList(),
        //        SelectedGroup = group.Name,
        //        Users = _dbContext.GroupUser
        //        .Where(gu => gu.GroupId == groupId)
        //        .Include(gu => gu.User)
        //        .Select(gu => gu.User)
        //        .ToList()
        //    };
        //    return View(model);
        //}

        
        public IActionResult UserAssignment(int groupId)
        {
            var group = _dbContext.Group.FirstOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                return RedirectToAction("GroupSelection");
            }

            var model = new GroupAssignmentViewModel
            {
                SelectedGroup = group.Name,
                Users = _dbContext.Users.ToList()
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult GetGroupUsers(int groupId)
        {
            var groupUsers = _dbContext.GroupUser
                .Include(gu => gu.User)
                .Include(gu => gu.Group)
                .Where(gu => gu.GroupId == groupId)
                .Select(gu => gu.User)
                .ToList();

            var users = groupUsers.Select(u => new { Id = u.Id, UserName = u.UserName }).ToList();

            return Json(new { users });
        }

        [HttpPost]
        public IActionResult AssignUsers(string selectedGroup, string[] userIds)
        {
            var groupId = _dbContext.Group.FirstOrDefault(x=>x.Name == selectedGroup).Id;
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
            return RedirectToAction("Index",new { groupId });
        }



        
    }
}
