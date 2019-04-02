using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using EFA.Models;

namespace EFA
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Test_EF();
        }

        void Test_EF()
        {
            var admin = new User
            {
                UserName = "admin",
                Password = "password",
                Admin = true,
                CreationDate = DateTime.Now
            };
            var db = new DBEntities();
            db.Users.Add(admin);
            db.SaveChanges();
            db.Dispose();
        }
    }
}
