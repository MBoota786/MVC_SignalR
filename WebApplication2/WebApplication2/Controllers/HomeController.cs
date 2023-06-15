using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    
    public class HomeController : Controller
    {
        userManagementEntities context;

        public HomeController()
        {
            if (context == null)
                context = new userManagementEntities();
        }


        [AllowAnonymous]
        public ActionResult Index()
        {

            return View();
        }

        [AllowAnonymous]
        public ActionResult AddProducts()
        {
            return View();
        }

        [Authorize]
        public ActionResult User()
        {
            return View();
        }

        //protected override void Dispose(bool disposing)
        //{
        //    context.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}
