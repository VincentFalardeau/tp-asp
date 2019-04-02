using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFA.Models;

namespace EFA.Controllers
{
    public class UsersController : Controller
    {
        private DBEntities DB = new DBEntities();

        protected override void Dispose(bool disposing)
        {
            DB.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Subscribe()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Subscribe(UserView userView)
        {
            if (ModelState.IsValid)
            {
                User userFound = DB.Users.Where(u => u.UserName == userView.UserName).FirstOrDefault();
                if (userFound != null)
                {
                    ModelState.AddModelError("UserName", "This username is already taken. Please choose another one.");
                    return View();
                }
                User user = new User
                {
                    UserName = userView.UserName,
                    Password = userView.Password,
                    Admin = false,
                    CreationDate = DateTime.Now
                };
                DB.Users.Add(user);
                DB.SaveChanges();
                return Redirect("Home");
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginView loginView)
        {
            if (ModelState.IsValid)
            {
                User userFound = DB.Users.Where(u => u.UserName == loginView.UserName).FirstOrDefault();
                if (userFound == null)
                {
                    ModelState.AddModelError("UserName", "This username does not exist.");
                    return View();
                }
                else
                {
                    if (userFound.Password != loginView.Password)
                    {
                        ModelState.AddModelError("password", "Wrong password");
                        return View();
                    }
                }
                OnlineUsers.AddSessionUser(userFound);
            }
            else
                return View();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            OnlineUsers.RemoveSessionUser();
            return RedirectToAction("Index", "Home");
        }
    }
}