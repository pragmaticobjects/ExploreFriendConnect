using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ExploreFriendConnect.Conf;
using ExploreFriendConnect.Auth;
using ExploreFriendConnect.Models;
using ExploreFriendConnect.Util;

namespace ExploreFriendConnect.Controllers
{
    public class HomeController : Controller
    {
        [RequireFriendConnectLogin]
        public ActionResult Index()
        {
            User currentUser = null;
            string fcAuthToken = string.Empty;
            if (Request.Cookies[ApplicationConfiguration.FriendConnectCookie] != null)
            {
                fcAuthToken = Request.Cookies[ApplicationConfiguration.FriendConnectCookie].Value;
            }

            if (!string.IsNullOrEmpty(fcAuthToken) )
            {
                currentUser = FriendConnectUtil.getViewer(fcAuthToken, true);
                Session["currentUser"] = currentUser;

                System.Collections.Hashtable ht = FriendConnectUtil.getViewerFriends(fcAuthToken,
                    0, 10);
            }
                        
            return View(currentUser);
        }
    }
}
