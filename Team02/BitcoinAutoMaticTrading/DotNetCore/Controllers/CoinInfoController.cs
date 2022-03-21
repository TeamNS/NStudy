using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace DotNetCore.Controllers
{
    public class CoinInfoController : Controller
    {
        //[Route("Home/Index")]
        public IActionResult CoinInfo()
        {
            //// 로그인 정보는 빼고 코인 데이터만 갱신하도록 분리해야 함.
            //List<UpbitAPI> apiAccount = new List<UpbitAPI>(); // 계정
            //Dictionary<String, String> Account = new Dictionary<string, string>(); // AccessKey, SecretKey 넣을 자료구조 추후에는 입력으로 넣어지도록

            //Account.Add("", ""); // 용환 키
            ////Account.Add("5X653IjqwRmPlUbIKVxaxX5y4zigwCrT26jkQYFN", "2pRxhDft2YZCiU8Z2gsYOIPecXpJgkFX1IPlI2Sq"); // 응준 키

            //List<List<Account>> ListAccount = new List<List<Account>>();

            //foreach (KeyValuePair<String, String> Keys in Account)
            //{
            //    UpbitAPI userAPI = new UpbitAPI(Keys.Key, Keys.Value);
            //    apiAccount.Add(userAPI);

            //    if (apiAccount[0].GetAccount() != null)
            //        ListAccount.Add(apiAccount[0].GetAccount());
            //}

            //// [0] 인덱스는 계정 정보 외에는 공용 데이터임.
            //apiAccount[0].SetMarketInfo(apiAccount[0].GetMarketInfo());

            //apiAccount[0].m_DicCoinCandleMinutes.Clear();
            //Thread.Sleep(1000);

            //apiAccount[0].UpdateCoinInfo(apiAccount[0]); // 1초마다 코인 정보 갱신
            ////apiAccount[0].print(ListAccount[0]);

            //Response.Headers.Add("Refresh", "0.1");
            return View(/*apiAccount[0].m_DicCoinCandleMinutes*/);
        }
    }
}
