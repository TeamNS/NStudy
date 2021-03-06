using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Possession() // 인자로 Account 객체가 오도록 해야 함. 
        {
            //List<Account> ListAccount = new List<Account>();
            //string AccessKey = "iyXWNzCSkTu2lX4DPYq6px7ARxG7qdH6eIV6jByG";

            //if (!m_AccountKey.ContainsKey(AccessKey))
            //{
            //    m_AccountKey.Add(AccessKey, "ZFN8Xrmx5zLHQBXtAwsk9IXwqArLkVXVLzbyYgJb");
            //    m_UpbitAPI.AddAccount(AccessKey, "ZFN8Xrmx5zLHQBXtAwsk9IXwqArLkVXVLzbyYgJb");
            //}

            //Account myAccount = m_UpbitAPI.GetAccount(AccessKey);
            //List<PossessionCoinInfo> PossessionCoinInfos = new List<PossessionCoinInfo>();
            //if (myAccount != null)
            //{
            //    if (m_UpbitAPI.GetAccount(myAccount.GetNoParam()) != null)
            //    {
            //        //m_UpbitAPI.SetAccountPossessionInfo(AccessKey);
            //        PossessionCoinInfos = m_UpbitAPI.GetAccount(myAccount.GetNoParam());
            //    }

            //}

            Response.Headers.Add("Refresh", "0.5");
            return View(/*PossessionCoinInfos*/);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
