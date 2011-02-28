using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ExploreFriendConnect.Conf;
using ExploreFriendConnect.Models;

namespace ExploreFriendConnect.Auth
{
    public class RequireFriendConnectLoginAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {            
            User user = null;
            string fcAuthToken = null;

            if (httpContext.Session["currentUser"] != null)
            {
                user = (User)httpContext.Session["currentUser"];
            }

            if (httpContext.Request.Cookies[ApplicationConfiguration.FriendConnectCookie] != null)
            {
                fcAuthToken = httpContext.Request.Cookies[ApplicationConfiguration.FriendConnectCookie].Value;
            }

            if (user == null && fcAuthToken == null)
            {                
                //httpContext.Response.Redirect("/Login", true);
                return false;
            }
            else if (user != null && fcAuthToken == null)
            {
                httpContext.Session.Remove("currentUser");
                //httpContext.Response.Redirect("/Logout", true);
                return false;
            }

            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //filterContext.Result = new RedirectResult("/Login");
            //base.HandleUnauthorizedRequest(filterContext);
            filterContext.HttpContext.Response.Redirect("/Login", true);
        }
    }
}