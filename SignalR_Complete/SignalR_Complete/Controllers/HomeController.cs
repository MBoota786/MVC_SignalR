using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SignalR_Complete.Data;
using SignalR_Complete.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SignalR_Complete.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get the signed-in user
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            // Get the groups that have some users inside
            var groups = _dbContext.GroupUser
                .Include(gu => gu.Group)
                .Select(gu => gu.Group)
                .ToList()
                .GroupBy(g => g.Name)
                .Select(g => g.FirstOrDefault())
                .ToList();

            var model = new IndexViewModel
            {
                Users = _dbContext.Users.ToList(),
                Groups = groups
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
