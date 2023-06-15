using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        userManagementEntities context = new userManagementEntities();
        public ActionResult Index()
        {
            return View();
        }
        //______________________ Users ____________________________
        #region User
        public ActionResult Users()
        {
            var list = context.users.ToList();
            return View(list);
        }
        #endregion

        //______________________ Roles ____________________________
        #region Roles
        public ActionResult Roles()
        {
            var list = context.roles.ToList();
            return View(list);
        }
        public PartialViewResult EditRole(int id)
        {
            var record = context.roles.Find(id);
            return PartialView("_editRole", record);
        }
        [HttpPost]
        public ActionResult EditRole(role model)
        {
            try
            {
                context.Entry(model).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Roles");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Roles");
            }
        }

        public ActionResult DeleteRole(int id)
        {
            try
            {
                var record = context.roles.Find(id);
                context.Entry(record).State = EntityState.Deleted;
                context.SaveChanges();
                return RedirectToAction("Roles");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Roles");
            }
        }

        #endregion

        //______________________ RoleAssign ____________________________
        public ActionResult UserRoles()
        {
            roleAssign_VM vm = new roleAssign_VM();
            vm.userRoleList = context.usersRoles.ToList();
            vm.userList = context.users.ToList();
            vm.roleList = context.roles.ToList();
            return View(vm);
        }
        public PartialViewResult AssignRole(int id)
        {
            var user = context.users.Find(id);
            var userrole = context.usersRoles.Where(x => x.userId == user.id).ToList();
            roleAssign_VM vm = new roleAssign_VM();
            vm.user = user;
            vm.userRoleList = userrole;
            vm.roleList = context.roles.ToList();
            return PartialView("_roleAssign", vm);
        }
        [HttpPost]
        public ActionResult AssignRole(roleAssign_VM model)
        {
            try
            {
                //___ remove old record
                var oldList = context.usersRoles.Where(x => x.userId == model.user.id).ToList();
                context.usersRoles.RemoveRange(oldList);
                context.SaveChanges();

                
                //___ insert selected record ___
                foreach (var item in model.checkObject)
                {
                    var newRecord = new usersRole()
                    {
                        userId = model.user.id,
                        roleId = item
                    };
                    context.usersRoles.Add(newRecord);
                    context.SaveChanges();
                }
                return RedirectToAction("UserRoles");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("UserRoles");
            }
        }


    }
}