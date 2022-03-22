using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Text;
using Newtonsoft.Json;

using System.Threading;




namespace DotNetCore
{
    public static class UpbitAPI
    {
        const int INPUT_COUNT = 5;

        private static Account m_CoinDataAccount = null;

        private static Dictionary<string, Account> m_DicAccounts = new Dictionary<string, Account>();
        private static Dictionary<String, String> m_AccountKey { get; set; } = new Dictionary<string, string>(); // AccessKey, SecretKey 넣을 자료구조 추후에는 입력으로 넣어지도록

        private static List<MarketInfo> m_ListMarketInfo;

        public static List<CandleMinute> m_ListCandleMinutes { get; set; } = new List<CandleMinute>();
        public static Dictionary<String, List<CandleMinute>> m_DicCoinCandleMinutes { get; set; } = new Dictionary<String, List<CandleMinute>>();
        public static Dictionary<String, List<CandleMinute>> m_DicPrevCoinCandleMinutes { get; set; } = new Dictionary<String, List<CandleMinute>>();


        public static void AddAccount(string AccessKey, string SecretKey)
        {
            Account newAccount = new Account(AccessKey, SecretKey);
            if(!m_DicAccounts.ContainsKey(AccessKey))
            {
                m_DicAccounts.Add(AccessKey, newAccount);
            }

            if (m_CoinDataAccount == null)
            {
                m_CoinDataAccount = new Account(AccessKey, SecretKey);
            }
        }

        public static Account GetAccount(string key)
        {
            if (key == null)
                return null;

            if (m_DicAccounts.ContainsKey(key) == false)
                return null;

           Account myAccount;
           bool hasValue = m_DicAccounts.TryGetValue(key, out myAccount);

           return myAccount;
        }

        public static Account GetCoinDataAccount()
        {
            if(m_CoinDataAccount != null)
            {
                return m_CoinDataAccount;
            }    

            return null;
        }

        public static void SetAccountPossessionInfo(string key)
        {
            if (m_DicAccounts.ContainsKey(key))
            {
                // 계정 보유 코인 정보 insert
            }
        }

        public static Dictionary<string, Account> GetAccountAll()
        {
            return m_DicAccounts;
        }

        public static void SetMarketInfo(List<MarketInfo> ListMarketInfo)
        {
            m_ListMarketInfo = ListMarketInfo;
        }

        public static void UpdateCoinInfo()
        {
            for (int i = 0; i < m_ListMarketInfo.Count; ++i)
            {
                string Str = m_ListMarketInfo[i].market;

                // 현재 특정 5개 종목 추출
                if (!Str.Equals("KRW-ICX") && !Str.Equals("KRW-REP") && !Str.Equals("KRW-WAVES") && !Str.Equals("KRW-SRM") && !Str.Equals("KRW-ETC"))
                    continue;

                List<CandleMinute> ListCandleMinute = UpbitAPI.GetCandleMinutes(m_CoinDataAccount.GetParam(), m_ListMarketInfo[i].market, UpbitAPI.MinuteUnit._1, DateTime.Now, 1);

                if (m_DicCoinCandleMinutes.ContainsKey(m_ListMarketInfo[i].market) == true)
                    m_DicCoinCandleMinutes[m_ListMarketInfo[i].market] = ListCandleMinute;
                else
                    m_DicCoinCandleMinutes.Add(m_ListMarketInfo[i].market, ListCandleMinute);            
            }
        }
        public static void print(List<PossessionCoinInfo> ListAccount)
        {
            for (int i = 0; i < ListAccount.Count; ++i) // 해당 계좌가 가지고 있는 코인 정보
                Console.WriteLine(ListAccount[i].currency + " | " + ListAccount[i].avg_buy_price + " | " + ListAccount[i].unit_currency);

            Console.WriteLine();

            foreach (KeyValuePair<String, List<CandleMinute>> CandleInfo in m_DicCoinCandleMinutes)
            {
                Console.WriteLine("코인 :" + CandleInfo.Key + " | 현재 거래 가격 : " + CandleInfo.Value[0].trade_price + " | 거래량 : " + CandleInfo.Value[0].candle_acc_trade_volume + " | 현재 시간 : " + CandleInfo.Value[0].candle_date_time_kst);
            }

            Console.WriteLine();
        }
        public static List<PossessionCoinInfo> GetAccount(NoParam noParam)
        {
            var data = noParam.Get("/v1/accounts", RestSharp.Method.Get);
            return JsonConvert.DeserializeObject<List<PossessionCoinInfo>>(data);
        }

        public static List<MarketInfo> GetMarketInfo(NoParam noParam)
        {
            var data = noParam.Get("/v1/market/all", RestSharp.Method.Get);
            return JsonConvert.DeserializeObject<List<MarketInfo>>(data);
        }

        public static List<CandleMinute> GetCandleMinutes(Param Param, string market, MinuteUnit unit, DateTime to = default(DateTime), int count = 1)
        {
            // 시세 캔들 조회 - 분(Minute) 캔들
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("market", market);
            parameters.Add("to", to.ToString("yyyy-MM-dd HH:mm:ss"));
            parameters.Add("count", count.ToString());
            var data = Param.Get(String.Join("", "/v1/candles/minutes/", (int)unit), parameters, RestSharp.Method.Get);
            return JsonConvert.DeserializeObject<List<CandleMinute>>(data);
        }

        public static RequestOrderInfo RequestOrderLimit(Param Param, string market, double volume, double price) // 지정가 매수 / 매도
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("market", market);
            parameters.Add("side", OrderSide.bid.ToString());
            parameters.Add("volume", volume.ToString());
            parameters.Add("price", price.ToString());
            parameters.Add("ord_type", "limit");

            var data = Param.Get("/v1/orders", parameters, RestSharp.Method.Post);
            return JsonConvert.DeserializeObject<RequestOrderInfo>(data);
        }
        public static RequestOrderInfo RequestOrderMarketBuy(Param Param, string market, double volume, double price) // 시장가 매수 
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("market", market);
            parameters.Add("side", OrderSide.bid.ToString());
            parameters.Add("volume", volume.ToString());
            parameters.Add("price", price.ToString());
            parameters.Add("ord_type", "price");

            var data = Param.Get("/v1/orders", parameters, RestSharp.Method.Post);
            return JsonConvert.DeserializeObject<RequestOrderInfo>(data);
        }
        public static RequestOrderInfo MakeOrderMarketSell(Param Param, string market, double volume, double price) // 시장가 매도
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("market", market);
            parameters.Add("side", OrderSide.ask.ToString());
            parameters.Add("volume", volume.ToString());
            parameters.Add("price", price.ToString());
            parameters.Add("ord_type", "market");

            var data = Param.Get("/v1/orders", parameters, RestSharp.Method.Post);
            return JsonConvert.DeserializeObject<RequestOrderInfo>(data);
        }

        public enum OrderSide
        {
            bid,
            ask
        }

        public enum Ord_Type
        {
            limit,
            price,
            market
        }
        public enum MinuteUnit
        {
            _1 = 1,
            _3 = 3,
            _5 = 5,
            _10 = 10,
            _15 = 15,
            _30 = 30,
            _60 = 60,
            _240 = 240
        }
    }
}