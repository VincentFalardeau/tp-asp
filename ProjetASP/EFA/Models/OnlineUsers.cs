using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFA.Models
{
    public static class OnlineUsers
    {
        public static void AddSessionUser(User user)
        {
            if (HttpRuntime.Cache["OnLineUsers"] == null)
                HttpRuntime.Cache["OnLineUsers"] = new List<User>();
            ((List<User>)HttpRuntime.Cache["OnLineUsers"]).Add(user);
            HttpContext.Current.Session["User"] = user;
            HttpContext.Current.Session.Timeout = 90;
        }
        public static void RemoveSessionUser()
        {
            ((List<User>)HttpRuntime.Cache["OnLineUsers"]).Remove(GetSessionUser());
            HttpContext.Current.Session.Abandon();
        }
        public static User GetSessionUser()
        {
            try
            {
                return (User)HttpContext.Current.Session["User"];
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static bool IsOnLine(int userId)
        {
            foreach (User user in (List<User>)HttpRuntime.Cache["OnLineUsers"])
            {
                if (user.Id == userId)
                    return true;
            }
            return false;
        }
    }

}