using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SignalR.Data;
using SignalR.Hubs;
using SignalR.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public readonly ApplicationDbContext _context;
        public readonly UserManager<AppUser> userManager;
        private readonly IHubContext<ChatHub> hubContext;

        public HomeController(ILogger<HomeController> logger
                                , UserManager<AppUser> userManager 
                                , ApplicationDbContext _context
                                , IHubContext<ChatHub> hubContext)
        {
            _logger = logger;
            this._context = _context;
            this.userManager = userManager;
            this.hubContext = hubContext;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await userManager.GetUserAsync(User);
            var s = await userManager.Users.Where(x=>x.Id != currentUser.Id).ToListAsync();
            return View(s);
        }

        public async Task<IActionResult> SelectedMessage(string receverId)
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentUserName = currentUser.UserName;
            }
            //var message = await _context.Message.Where(x=>x.ReceverId == receverId && x.SenderId == currentUser.Id ).ToListAsync();
            var message = await _context.Message.ToListAsync();
            var v = new ViewModel();
            v.Messages = message;
            v.RecevireId = receverId;
            v.SenderId = currentUser.Id;
            v.userName = currentUser.ToString();
            await hubContext.Clients.User(receverId).SendAsync("ReceiveMessage", User.Identity.Name, message);
            return View(v);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Message model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _context.Message.AddAsync(model);
                    await _context.SaveChangesAsync();
                    await hubContext.Clients.User(model.ReceverId).SendAsync("ReceiveMessage", User.Identity.Name, model.Text);
                    return Ok();
                }
            }
            catch (Exception ms)
            {
                var message = ms.Message;
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
