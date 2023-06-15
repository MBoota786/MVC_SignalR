using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SignalR.Data;
using SignalR.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace SignalR.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public readonly ApplicationDbContext _context;
        public readonly UserManager<AppUser> userManager;

        public HomeController(ILogger<HomeController> logger
                                , UserManager<AppUser> userManager 
                                , ApplicationDbContext _context)
        {
            _logger = logger;
            this._context = _context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentUserName = currentUser.UserName;
            }
            var message = await _context.Message.ToListAsync();
            return View(message);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Message m)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    m.UserName = User.Identity.Name;
                    var sender = await userManager.GetUserAsync(User);
                    m.UserId = sender.Id;
                    await _context.AddAsync(m);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return Error();
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
