using EFA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFA.Controllers
{
    public class CategoriesController : Controller
    {

        private DBEntities DB = new DBEntities();
        // GET: Categorie
        public ActionResult Index()
        {
            return View(DB.Categories.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CategorieView categorieView)
        {

            User loggedUser = OnlineUsers.GetSessionUser();

            Category categorie = new Category
            {
                Name = categorieView.Name

            };

            DB.Categories.Add(categorie);

            DB.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int Id)
        {
            CategorieView category = new CategorieView(DB.Categories.Where(x => x.Id == Id).FirstOrDefault());
            return View(category);
        }

        public ActionResult Delete(int Id)
        {
            CategorieView category = new CategorieView(DB.Categories.Where(x => x.Id == Id).FirstOrDefault());
            return View(category);
        }


        [HttpPost]
        public ActionResult Edit(CategorieView categorieView)
        {

            if (categorieView != null)
            {
                Category NewCategorie = new Category
                {
                    Id = categorieView.Id,
                    Name = categorieView.Name

                };
                User loggedUser = OnlineUsers.GetSessionUser();

                Category categorie = DB.Categories.Where(x => x.Id == categorieView.Id).FirstOrDefault();


                if (categorie != null)
                {
                    categorie.Update(NewCategorie);
                    DB.Entry(categorie).State = System.Data.Entity.EntityState.Modified;
                    DB.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(CategorieView categorieView)
        {

            if (categorieView != null)
            {

                User loggedUser = OnlineUsers.GetSessionUser();

                Category categorie = new Category
                {
                    Name = categorieView.Name

                };

                Category categoryToDelete = DB.Categories.Where(x => x.Id == categorieView.Id).FirstOrDefault();
                if (categoryToDelete != null)
                {
                    foreach (Bookmark bookmark in DB.Bookmarks.Where(b => b.CategoryId == categoryToDelete.Id))
                    {
                        bookmark.CategoryId = 0;
                        DB.Entry(bookmark).State = System.Data.Entity.EntityState.Modified;
                    }
                    DB.Entry(categoryToDelete).State = System.Data.Entity.EntityState.Deleted;
                    DB.SaveChanges();
                }
            }

          
            return RedirectToAction("Index");
        }
    }
}