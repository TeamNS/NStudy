using DotNetCoreService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreService.Controllers
{
    public class SocialController : Controller
    {
        public IActionResult GetFriendsList(Int64 UserUID)
        {
            var context = HttpContext.RequestServices.GetService(typeof(DBConnection)) as DBConnection;
            List<Friend> list = context.GetFriendsListAll(UserUID);

            return View(list);
        }

        public IActionResult AddFriends(Int64 UserUID, Int64 FriendUID)
        {
            var context = HttpContext.RequestServices.GetService(typeof(DBConnection)) as DBConnection;
            Int32 ret = context.AddFriends(UserUID, FriendUID);

            if (ret != 0)
            {
                return View(ret);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
