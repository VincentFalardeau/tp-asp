using EFA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFA.Controllers
{
    public class BookmarksController : Controller
    {



    

       private DBEntities DB = new DBEntities();

        #region Sort and filters
        private void InitSessionSortAndFilter()
        {
            if (Session["BookmarkSortBy"] == null)
            {
                Session["BookmarkSortBy"] = "Name";
                Session["BookmarkSortAscendant"] = true;
            }

            if (Session["BookmarkFilterByOwnership"] == null)
            {
                Session["BookmarkFilterByOwnership"] = -1;
            }

            if (Session["BookmarkFilterByCategory"] == null)
            {
                Session["BookmarkFilterByCategory"] = -1;
            }
        }

        public ActionResult Sort(string by)
        {
            if (by == (string)Session["BookmarkSortBy"])
                Session["BookmarkSortAscendant"] = !(bool)Session["BookmarkSortAscendant"];
            else
                Session["BookmarkSortAscendant"] = true;

            Session["BookmarkSortBy"] = by;
            return RedirectToAction("Index");
        }

        public ActionResult FilterOwnership(int Ownership)
        {
            Session["BookmarkFilterByOwnership"] = Ownership;
            return RedirectToAction("Index");
        }

        public ActionResult FilterCategory(int Category)
        {
            Session["BookmarkFilterByCategory"] = Category;
            return RedirectToAction("Index");
        }
        #endregion





        // GET: Bookmarks
        public ActionResult Index()
        {
            User loggedUser = OnlineUsers.GetSessionUser();
            InitSessionSortAndFilter();
            var Query = DB.BookmarkList(loggedUser, (string)Session["BookmarkSortBy"], (bool)Session["BookmarkSortAscendant"]);

            

            int categorieFilter = (int)Session["BookmarkFilterByCategory"];
            int ownerShipFilter = (int)Session["BookmarkFilterByOwnership"];

            ViewBag.Categories = DB.Categories;
            ViewBag.Users = DB.Users;
            ViewBag.CurrentUser = loggedUser;

            if (categorieFilter != -1)
            {
                string categorie_name = DB.Categories.Where(x => x.Id == categorieFilter).FirstOrDefault().Name.ToString();
                Query = Query.Where(x => x.CategoryName == categorie_name).ToList();
            }

            if (ownerShipFilter != -1)
            {
                if(ownerShipFilter == -2)
                {
                    Query = Query.Where(x => x.Shared).ToList();
                }else if (ownerShipFilter == -3)
                {
                    Query = Query.Where(x => !x.Shared).ToList();
                }else
                {
                    Query = Query.Where(x => x.OwnerId == ownerShipFilter && !x.Shared).ToList();
                }
            }


            return View(Query);
        }


        public ActionResult Create() {
            
            return View();
        }

        [HttpPost]
        public ActionResult Create(BookmarkView bookmarkview) {

            User loggedUser = OnlineUsers.GetSessionUser();

            Bookmark bookmark = Bookmark.FromBookmarkView(bookmarkview);
                        
            DB.Bookmarks.Add(bookmark);

            DB.SaveChanges();
            return RedirectToAction("Index");
        }



        public ActionResult Edit(BookmarkView bookmarkView)
        {
            if (ModelState.IsValid)
            {
                Bookmark bookmark = new Bookmark();
                bookmark.Id = bookmarkView.Id;
                bookmark.Name = bookmarkView.Name;
                bookmark.Shared = bookmarkView.Shared;
                bookmark.Url = bookmarkView.Url;
                bookmark.UserId = DB.Users.Where(x => x.UserName == bookmarkView.OwnerName).FirstOrDefault().Id;
                bookmark.CategoryId = Bookmark.GetCategoryIdFromBookmarkView(bookmarkView);


                DB.Update(bookmark);

                DB.SaveChanges();
            }
            return View(bookmarkView);
        }

        public ActionResult Details(BookmarkView bookmarkView)
        {
            return View(bookmarkView);
        }

        public ActionResult Delete(BookmarkView bookmarkView)
        {
            Bookmark bookmark = new Bookmark();
            bookmark.Id = bookmarkView.Id;
            bookmark.Name = bookmarkView.Name;
            bookmark.Shared = bookmarkView.Shared;
            bookmark.Url = bookmarkView.Url;
            bookmark.UserId = DB.Users.Where(x => x.UserName == bookmarkView.OwnerName).First().Id;
            bookmark.CategoryId = Bookmark.GetCategoryIdFromBookmarkView(bookmarkView);

            return View(bookmark);
        }

        [HttpPost]
        public ActionResult Delete(Bookmark bookmark)
        {
            bookmark = DB.Bookmarks.Where(x => x.Id == bookmark.Id).First();
            DB.Delete(bookmark);

            DB.SaveChanges();

            return RedirectToAction("Index");
        }

       

        //[HttpPost]
        //public ActionResult Edit(Bookmark bookmark) {
        //    DB.Bookmarks.Update(bookmark);
        //    return RedirectToAction("Index");
        //}

        //public ActionResult Details(int Id) { Bookmark bookmark = DB.Bookmarks.Get(Id); return View(bookmark); }

        //public ActionResult Delete(int Id) { Bookmark bookmark = DB.Bookmarks.Get(Id); return View(bookmark); }

        //[HttpPost] public ActionResult Delete(Bookmark bookmark) { DB.Bookmarks.Delete(bookmark.Id); return RedirectToAction("Index"); }

    }


}

