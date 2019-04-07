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

        // GET: Bookmarks
        public ActionResult Index()
        {
            List<BookmarkView> bookmarkViews = BookmarkView.GetDBCollection();
            return View(bookmarkViews);
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
                bookmark.UserId = DB.Users.Where(x => x.UserName == bookmarkView.OwnerName).First().Id;
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

