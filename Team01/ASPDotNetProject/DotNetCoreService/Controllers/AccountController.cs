using DotNetCoreService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreService.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// 로그인
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            return View(JwtManager.GenerateToken(user.UserName));


        }


        /// <summary>
        /// 계정생성
        /// </summary>
        /// <returns></returns>
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User model)
        {
            //if(ModelState.IsValid)
            //{
            //    // Valid Check 
            //    //using (var db = new Asp())
            //    //{

            //    //}

            //    return RedirectToAction("Index", "Home");
            //}

            //return View(model);


            if (ModelState.IsValid)
            {
                var context = HttpContext.RequestServices.GetService(typeof(DBConnection)) as DBConnection;
                Int32 ret = context.RegistUserInfo(model);

                if(ret != 0)
                {
                    return View(model);
                }
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
    }
}
