using System;
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
    public class FriendsController : Controller
    {
        [RequireFriendConnectLogin]
        public ActionResult Index()
        {
            return View();
        }
    }
}
