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
            User userFound = DB.Users.Where(u => u.UserName == userView.UserName).FirstOrDefault();
            if (userFound != null)
            {
                ModelState.AddModelError("UserName", "This username is already taken. Please choose another one.");
                return View();

            }
            if (userView.Sex == SexType.Null)
            {
                ModelState.AddModelError("Sex", "You need to indicate your gender");
                return View();

            }

            if (ModelState.IsValid)
            {
                User user = new User
                {
                    UserName = userView.UserName,
                    FirstName = userView.FirstName,
                    LastName = userView.LastName,
                    Password = userView.Password,
                    Admin = false,
                    CreationDate = DateTime.Now,
                    Sex = userView.Sex,
                    BirthDate = userView.BirthDate,
                    Email = userView.Email
                };

                DB.Users.Add(user);
                DB.SaveChanges();

                LogUser(DB.Users.Where(u => u.UserName == userView.UserName).FirstOrDefault());

                return RedirectToAction("Index", "Bookmarks");
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
                LogUser(userFound);
            }
            else
            {
                return View();
            }
                

            return RedirectToAction("Index", "Bookmarks");
        }

        public void LogUser(User user)
        {
            ViewBag.UserName = user.UserName;
            OnlineUsers.AddSessionUser(user);
            ViewBag.UserName = OnlineUsers.GetSessionUser().UserName;
        }

        public ActionResult Profile(User user)
        {
            User userFound = DB.Users.Where(u => u.UserName == user.UserName).FirstOrDefault();

            if (userFound != null && userFound.UserName != OnlineUsers.GetSessionUser().UserName)
            {
                ModelState.AddModelError("UserName", "This username is already taken. Please choose another one.");
            }
            if (user.Sex == SexType.Null)
            {
                ModelState.AddModelError("Sex", "You need to indicate your gender");
            }

            if(user.Password == "")
            {
                ModelState.AddModelError("Password", "A password is required");
            }

            bool invalidEmail = false;
            try
            {
                invalidEmail = new System.Net.Mail.MailAddress(user.Email) == null;
            }
            catch(Exception e)
            {
                invalidEmail = true;
            }
            
            if (invalidEmail)
            {
                ModelState.AddModelError("Email", "Invalid email address");
            }

            if (ModelState.IsValid)
            {
                DB.Update(user);

                OnlineUsers.UpdateSessionUser(DB.Users.Find(user.Id));
            }

            return View(OnlineUsers.GetSessionUser());
        }

        public ActionResult Logout()
        {
            OnlineUsers.RemoveSessionUser();
            return RedirectToAction("Login", "Users");
        }
    }
}