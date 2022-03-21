using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Net;
using System.IO;

using Newtonsoft.Json;
using System.Threading;
using System.Diagnostics;

// 회원가입은 의미가 있는지 -> 있다면 AccessKey, SecretKey 를 DB에 넣어놔야 함 -> GetAccount() 검증시 존재할 경우 DB에 넣는다. Load도 같은 방식
// LIst<List<>> 구조에서 List<> 구조로 갈 수 있는 방법 찾아봐야 함 -> Get 구조가 변경되야 함.

namespace BitcoinAutoMaticTrading
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
           // update 로직으로 빼야 함
           List<UpbitAPI> apiAccount = new List<UpbitAPI>(); // 각 유저의 계정들 담을 자료구조
           
           Dictionary<String, String> Account = new Dictionary<string, string>(); // AccessKey, SecretKey 넣을 자료구조 추후에는 입력으로 넣어지도록
           Account.Add("iyXWNzCSkTu2lX4DPYq6px7ARxG7qdH6eIV6jByG", "ZFN8Xrmx5zLHQBXtAwsk9IXwqArLkVXVLzbyYgJb"); // 용환 키
           Account.Add("5X653IjqwRmPlUbIKVxaxX5y4zigwCrT26jkQYFN", "2pRxhDft2YZCiU8Z2gsYOIPecXpJgkFX1IPlI2Sq"); // 응준 키
           
           List<List<Account>> ListAccount = new List<List<Account>>();
           
           foreach (KeyValuePair<String, String> Keys in Account)
           {
               UpbitAPI userAPI = new UpbitAPI(Keys.Key, Keys.Value);
               apiAccount.Add(userAPI);
           
               if(apiAccount[0].GetAccount() != null)
                   ListAccount.Add(apiAccount[0].GetAccount());
           }
           
           // [0] 인덱스는 계정 정보 외에는 공용 데이터임.
           apiAccount[0].SetMarketInfo(apiAccount[0].GetMarketInfo());
           
           while (true)
           {
               Thread.Sleep(1000);
           
               apiAccount[0].UpdateCoinInfo(apiAccount[0]); // 1초마다 코인 정보 갱신
               apiAccount[0].print(ListAccount[0]);
           }
        }

        // 웹 띄울 시 필요
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

    }
}
