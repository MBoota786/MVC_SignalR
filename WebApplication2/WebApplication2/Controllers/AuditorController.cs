using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Controllers
{
    [Authorize(Roles = "Auditor")]
    public class AuditorController : Controller
    {
        // GET: Audiotor
        public ActionResult Index()
        {
            return View();
        }
    }
}