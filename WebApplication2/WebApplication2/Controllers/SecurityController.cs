using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class SecurityController : Controller
    {
        // GET: Security
        userManagementEntities context = new userManagementEntities();
        
        
        public ActionResult Registor()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registor(user model)
        {
            context.users.Add(model);
            context.SaveChanges();
            return RedirectToAction("Index","Home");
        }
        public ActionResult Login()
        {
            return View();
        }
      
        [HttpPost]
        public ActionResult Login(user model)
        {
            var result = context.users.Where(x=>x.email == model.email && x.password==model.password);
            if (result.ToList().Count > 0)
            {
                ViewBag.login = "login Successfuly";
                FormsAuthentication.SetAuthCookie(result.FirstOrDefault().name, false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.login = "Login Fail";
                return View();
            }
        }


        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Home");
        }
    }
}