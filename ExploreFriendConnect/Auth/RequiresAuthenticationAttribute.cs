using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ExploreFriendConnect.Conf;
using ExploreFriendConnect.Models;

namespace ExploreFriendConnect.Auth
{
    public class RequiresAuthenticationAttribute : ActionFilterAttribute
    {                
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            HttpContextBase context = filterContext.HttpContext;
            
            User user = null;
            string fcAuthToken = null;

            if (filterContext.HttpContext.Session["currentUser"] != null)
            {
                user = (User)filterContext.HttpContext.Session["currentUser"];
            }

            if (context.Request.Cookies[ApplicationConfiguration.FriendConnectCookie] != null)
            {
                fcAuthToken = context.Request.Cookies[ApplicationConfiguration.FriendConnectCookie].Value;                
            }

            if (user == null && fcAuthToken == null)
            {
                filterContext.HttpContext.Response.Redirect("/Login", true);
            }            
            else if (user != null && fcAuthToken == null)
            {
                context.Session.Remove("currentUser");
                filterContext.HttpContext.Response.Redirect("/Logout", true);
            }                            
        }
    }
}