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
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public readonly ApplicationDbContext _context;
        public readonly UserManager<AppUser> userManager;

        //private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<ChatHub> _hubContext;

        public HomeController(ILogger<HomeController> logger
                                , UserManager<AppUser> userManager 
                                , ApplicationDbContext _context
                                ,IHubContext<ChatHub> hubContext)
        {
            _logger = logger;
            this._context = _context;
            this.userManager = userManager;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(MessageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var sender = await userManager.GetUserAsync(User);
                var receiver = await userManager.FindByNameAsync(model.Receiver);

                if (receiver != null)
                {
                    await _hubContext.Clients.User(receiver.Id).SendAsync("ReceiveMessage", sender.UserName, model.Message);
                }

                return RedirectToAction("Index", "Home");
            }

            // Handle validation errors
            return View(model);
        }



        public async Task<IActionResult> SelectedMessage(string senderId)
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentUserName = currentUser.UserName;
            }
            var message = await _context.Message.Where(x=>x.SenderId == senderId && x.UserId == currentUser.Id).ToListAsync();
            var v = new ViewModel();
            v.Message = message;
            v.SenderId = senderId;
            v.userName = currentUser.ToString();
            return View(v);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Message message)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    message.UserName = User.Identity.Name;
                    var sender = await userManager.GetUserAsync(User);
                    message.UserId = sender.Id;
                    await _context.Message.AddAsync(message);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
            }
            catch (Exception ms)
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
