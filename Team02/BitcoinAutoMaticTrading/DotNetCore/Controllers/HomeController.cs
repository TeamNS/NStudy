using DotNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;


namespace DotNetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        Thread calcualteThread = null;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LoginPage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string ID, string PW, string AccessKey, string SecretKey)
        {
            if(ID == null || PW == null || AccessKey == null || SecretKey == null)
                return RedirectToAction("Possession", "Home"); // 다 채우지 않았다는 메시지를 보내야 함.

            //var context =  HttpContext.RequestServices.GetService(typeof(DBConnector)) as DBConnector;
            // 바인딩 방법 찾아봐야 함.
            DBConnector connecter = new DBConnector();
            bool result = connecter.AccountLogin(ID, PW, AccessKey, SecretKey);

            if(result == false)
                return RedirectToAction("LoginPage", "Home"); // 로그인 실패 띄워줘야 함

            List<string> testList = new List<string>();

            HttpContext.Session.SetString("Access", AccessKey);

            return RedirectToAction("Possession", "Home");
        }

        [HttpPost]
        public IActionResult SignUp(string ID, string PW, string AccessKey, string SecretKey)
        {
            if (ID == null || PW == null || AccessKey == null || SecretKey == null)
                return RedirectToAction("LoginPage", "Home"); // 다 채우지 않았다는 메시지를 보내야 함.

            //var context = HttpContext.RequestServices.GetService(typeof(DBConnector)) as DBConnector;
            DBConnector connecter = new DBConnector();
            bool result = connecter.AccountSignUp(ID, PW, AccessKey, SecretKey);

            return RedirectToAction("LoginPage", "Home"); // 회원 가입 완료 문구 띄워줘야 함
        }

        public IActionResult CoinInfo()
        {
            Account CoinDataAccount = UpbitAPI.GetCoinDataAccount();
            if(CoinDataAccount != null)
            {
                UpbitAPI.SetMarketInfo(UpbitAPI.GetMarketInfo(CoinDataAccount.GetNoParam()));
            }

            Thread.Sleep(1000);

            UpbitAPI.UpdateCoinInfo(); // 1초마다 코인 정보 갱신

            Response.Headers.Add("Refresh", "0.1");
            return View(UpbitAPI.m_DicCoinCandleMinutes);
        }

        public IActionResult AutoTradingRecord()
        {
            string AccessKey = HttpContext.Session.GetString("AccessKey");
            Account Account = UpbitAPI.GetAccount(AccessKey);

            //var error = m_UpbitAPI.MakeOrderMarketSell(Account.GetParam(), "KRW-ICX", 10 ,10000); // MakeOrderMarketSell RequestOrderMarketBuy
            var error = UpbitAPI.RequestOrderMarketBuy(Account.GetParam(), "KRW-ICX", 100, 10500);
            //var error = m_UpbitAPI.RequestOrderLimit(Account.GetParam(), "KRW-ICX", 15, 5000);

            // 현재 계정의 자동 매매 현황을 불러온다.
            return View();
        }

        public IActionResult CheckAutoTrading(string button)
        {
            string AccessKey = HttpContext.Session.GetString("AccessKey");

            if (button == "on")
            {
                if (calcualteThread == null)
                {
                    calcualteThread = new Thread(DoAutoTrading);
                }

                calcualteThread.Start();

                TempData["buttonval"] = "자동매매 시작";
                return RedirectToAction("AutoTradingRecord");
            }
            else
            {
                if (calcualteThread != null) // 생성한 스레드가 null 이다.
                {
                    calcualteThread.Abort();
                }

                TempData["buttonval"] = "자동매매 중지";
                return RedirectToAction("Privacy", "Account");
            }
        }

        public void DoAutoTrading()
        {
            while (true)
            {
                Thread.Sleep(5000);
            }
        }

        public IActionResult Possession() // 인자로 Account 객체가 오도록 해야 함. 
        {
            var AccessKey = HttpContext.Session.GetString("AccessKey");

            Account myAccount = UpbitAPI.GetAccount(AccessKey);
            List<PossessionCoinInfo> PossessionCoinInfos = new List<PossessionCoinInfo>();
            if (myAccount != null)
            {
                if(UpbitAPI.GetAccount(myAccount.GetNoParam())!= null)
                {
                    //m_UpbitAPI.SetAccountPossessionInfo(AccessKey);
                    PossessionCoinInfos = UpbitAPI.GetAccount(myAccount.GetNoParam());
                }
            }

            Response.Headers.Add("Refresh", "0.5");
            return View(PossessionCoinInfos);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
