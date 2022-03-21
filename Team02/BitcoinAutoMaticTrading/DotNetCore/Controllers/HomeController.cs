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

namespace DotNetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UpbitAPI m_UpbitAPI { get; set; } = new UpbitAPI(); // 각 유저의 계정들 담을 자료구조
        public UpbitAPI GetUpbitAPI()
        {
            return m_UpbitAPI;
        }
        private Dictionary<String, String> m_AccountKey { get; set; } = new Dictionary<string, string>(); // AccessKey, SecretKey 넣을 자료구조 추후에는 입력으로 넣어지도록

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

            //if(result == false)
            //

            return RedirectToAction("LoginPage", "Home"); // 회원 가입 완료 문구 띄워줘야 함
        }

        public IActionResult CoinInfo()
        {
            UpbitAPI a = GetUpbitAPI();

            string AccessKey = "";

            if (!m_AccountKey.ContainsKey(AccessKey))
            {
                m_AccountKey.Add(new string(""), new string(""));
                m_UpbitAPI.AddAccount(AccessKey, "");
            }

            // 공용 데이터 용
            Account showAccount = m_UpbitAPI.GetAccount(AccessKey);
            if (null != showAccount)
            {
                m_UpbitAPI.SetMarketInfo(m_UpbitAPI.GetMarketInfo(showAccount.GetNoParam()));
            }

            //m_UpbitAPI.m_DicPrevCoinCandleMinutes.Clear();
            //m_UpbitAPI.m_DicPrevCoinCandleMinutes = m_UpbitAPI.m_DicCoinCandleMinutes;
            //m_UpbitAPI.m_DicCoinCandleMinutes.Clear();

            Thread.Sleep(1000);

            m_UpbitAPI.UpdateCoinInfo(m_UpbitAPI); // 1초마다 코인 정보 갱신

            Response.Headers.Add("Refresh", "0.1");
            return View(m_UpbitAPI.m_DicCoinCandleMinutes);
        }

        public IActionResult AutoTradingRecord()
        {
            string AccessKey = "";

            if (!m_AccountKey.ContainsKey(AccessKey))
            {
                m_AccountKey.Add(AccessKey, "");
                m_UpbitAPI.AddAccount(AccessKey, "");
            }

            var Account = m_UpbitAPI.GetAccount(AccessKey);

            //var error = m_UpbitAPI.MakeOrderMarketSell(Account.GetParam(), "KRW-ICX", 10 ,10000); // MakeOrderMarketSell RequestOrderMarketBuy
            var error = m_UpbitAPI.RequestOrderMarketBuy(Account.GetParam(), "KRW-ICX", 100, 10500);
            //var error = m_UpbitAPI.RequestOrderLimit(Account.GetParam(), "KRW-ICX", 15, 5000);

            // 현재 계정의 자동 매매 현황을 불러온다.
            return View();
        }

        public IActionResult CheckAutoTrading(string button)
        {
            string AccessKey = "";

            if (!m_AccountKey.ContainsKey(AccessKey))
            {
                m_AccountKey.Add(AccessKey, "");
                m_UpbitAPI.AddAccount(AccessKey, "");
            }

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
                if (!m_AccountKey.ContainsKey(""))
                {
                    m_AccountKey.Add("", "");
                    m_UpbitAPI.AddAccount("", "");
                }
            }
        }

        public IActionResult Possession() // 인자로 Account 객체가 오도록 해야 함. 
        {
            List<Account> ListAccount = new List<Account>();
            string AccessKey = "";

            if (!m_AccountKey.ContainsKey(AccessKey))
            {
                m_AccountKey.Add(AccessKey, "");
                m_UpbitAPI.AddAccount(AccessKey, "");
            }

            Account myAccount = m_UpbitAPI.GetAccount(AccessKey);
            List<PossessionCoinInfo> PossessionCoinInfos = new List<PossessionCoinInfo>();
            if (myAccount != null)
            {
                if(m_UpbitAPI.GetAccount(myAccount.GetNoParam())!= null)
                {
                    //m_UpbitAPI.SetAccountPossessionInfo(AccessKey);
                    PossessionCoinInfos = m_UpbitAPI.GetAccount(myAccount.GetNoParam());
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
