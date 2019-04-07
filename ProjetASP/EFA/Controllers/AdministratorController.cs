using EFA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFA.Controllers
{
    public class AdministratorController : Controller
    {

        #region Filters
        private void InitSessionSortAndFilter()
        {
            if (Session["UserSortBy"] == null)
            {
                Session["UserSortBy"] = "FirstName";
                Session["UserSortAscendant"] = true;
            }

            if (Session["UserFilterBySex"] == null)
            {
                Session["UserFilterBySex"] = -1;
            }
        }
        public ActionResult Sort(string by)
        {
            if (by == (string)Session["UserSortBy"])
                Session["UserSortAscendant"] = !(bool)Session["UserSortAscendant"];
            else
                Session["UserSortAscendant"] = true;

            Session["UserSortBy"] = by;
            return RedirectToAction("UsersList", "Administrator");
        }
        public ActionResult FilterBySex(int sex)
        {
            Session["UserFilterBySex"] = sex;
            return RedirectToAction("UsersList", "Administrator");
        }
         #endregion


        // GET: Admin
        public ActionResult Index()
        {
            return RedirectToAction("UsersList", "Administrator");
        }

        private DBEntities db = new DBEntities();
        public ActionResult UsersList()
        {

            InitSessionSortAndFilter();
            List<User> users = ToFilterList((string)Session["UserSortBy"], (bool)Session["UserSortAscendant"]);
            int sexFilter = (int)Session["UserFilterBySex"];
            if (sexFilter != -1)
                return View(users.Where(c => c.Sex == sexFilter).ToList());
            return View(users);
        }



        public List<User> ToFilterList(string columnName, bool ascending)
        {
            DBEntities DB = new DBEntities();
            switch (columnName)
            {
                case "FirstName":
                    if (ascending)
                        return DB.Users.ToList().OrderBy(i => i.FirstName).ToList();
                    else
                        return DB.Users.ToList().OrderByDescending(i => i.FirstName).ToList();

                case "CreationDate":
                    if (ascending)
                        return DB.Users.ToList().OrderBy(i => i.CreationDate).ToList();
                    else
                        return DB.Users.ToList().OrderByDescending(i => i.CreationDate).ToList();

                case "BirthDate":
                    if (ascending)
                        return DB.Users.ToList().OrderBy(i => i.BirthDate).ToList();
                    else
                        return DB.Users.ToList().OrderByDescending(i => i.BirthDate).ToList();

                case "Sex":
                    if (ascending)
                        return DB.Users.ToList().OrderBy(i => i.Sex).ToList();
                    else
                        return DB.Users.ToList().OrderByDescending(i => i.Sex).ToList();

                default:
                    return DB.Users.ToList();
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Delete(UserView userView)
        {
            DBEntities DB = new DBEntities();
            return View(DB.Users.Where(x => x.Id == userView.Id).Include(x => x.Bookmarks).FirstOrDefault());
        }
    }
}