using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Text;
using Newtonsoft.Json;

using System.Threading;




namespace DotNetCore
{
    public class UpbitAPI
    {
        const int INPUT_COUNT = 5;

        private Account m_showAccount = null;

        private Dictionary<string, Account> m_DicAccounts = new Dictionary<string, Account>();

        private List<MarketInfo> m_ListMarketInfo;

        public List<CandleMinute> m_ListCandleMinutes { get; set; } = new List<CandleMinute>();
        public Dictionary<String, List<CandleMinute>> m_DicCoinCandleMinutes { get; set; } = new Dictionary<String, List<CandleMinute>>();
        public Dictionary<String, List<CandleMinute>> m_DicPrevCoinCandleMinutes { get; set; } = new Dictionary<String, List<CandleMinute>>();


        public UpbitAPI()
        {
        }

        public void AddAccount(string AccessKey, string SecretKey)
        {
            // 존재하는 계정인지 확인해야 함.

            Account newAccount = new Account(AccessKey, SecretKey);
            m_DicAccounts.Add(AccessKey, newAccount);

            if (m_showAccount == null)
            {
                m_showAccount = new Account(AccessKey, SecretKey);
            }
        }

        public Account GetAccount(string key)
        {
            if (m_DicAccounts.ContainsKey(key))
            {
                Account value;
                bool hasValue = m_DicAccounts.TryGetValue(key, out value);
                return value;
            }

            return null;
        }

        public void SetAccountPossessionInfo(string key)
        {
            if (m_DicAccounts.ContainsKey(key))
            {
                // 계정 보유 코인 정보 insert
            }
        }

        public Dictionary<string, Account> GetAccountAll()
        {
            return m_DicAccounts;
        }

        public void SetMarketInfo(List<MarketInfo> ListMarketInfo)
        {
            m_ListMarketInfo = ListMarketInfo;
        }

        public void UpdateCoinInfo(UpbitAPI api)
        {
            for (int i = 0; i < m_ListMarketInfo.Count; ++i)
            {
                string Str = m_ListMarketInfo[i].market;

                // 현재 특정 5개 종목 추출
                if (!Str.Equals("KRW-ICX") && !Str.Equals("KRW-REP") && !Str.Equals("KRW-WAVES") && !Str.Equals("KRW-SRM") && !Str.Equals("KRW-ETC"))
                    continue;

                //if (Str.Substring(0, 3) != "KRW") // 원화 마켓에 있는 데이터만 긁어옴
                //    continue;

                //if (m_DicCoinCandleMinutes.Count >= INPUT_COUNT)
                //    break;


                m_DicCoinCandleMinutes.Add(m_ListMarketInfo[i].market, api.GetCandleMinutes(m_showAccount.GetParam(), m_ListMarketInfo[i].market, UpbitAPI.MinuteUnit._1, DateTime.Now, 1));
            }
        }
        public void print(List<PossessionCoinInfo> ListAccount)
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
        public List<PossessionCoinInfo> GetAccount(NoParam noParam)
        {
            var data = noParam.Get("/v1/accounts", RestSharp.Method.Get);
            return JsonConvert.DeserializeObject<List<PossessionCoinInfo>>(data);
        }

        public List<MarketInfo> GetMarketInfo(NoParam noParam)
        {
            var data = noParam.Get("/v1/market/all", RestSharp.Method.Get);
            return JsonConvert.DeserializeObject<List<MarketInfo>>(data);
        }

        public List<CandleMinute> GetCandleMinutes(Param Param, string market, MinuteUnit unit, DateTime to = default(DateTime), int count = 1)
        {
            // 시세 캔들 조회 - 분(Minute) 캔들
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("market", market);
            parameters.Add("to", to.ToString("yyyy-MM-dd HH:mm:ss"));
            parameters.Add("count", count.ToString());
            var data = Param.Get(String.Join("", "/v1/candles/minutes/", (int)unit), parameters, RestSharp.Method.Get);
            return JsonConvert.DeserializeObject<List<CandleMinute>>(data);
        }

        public RequestOrderInfo RequestOrderLimit(Param Param, string market, double volume, double price) // 지정가 매수 / 매도
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
        public RequestOrderInfo RequestOrderMarketBuy(Param Param, string market, double volume, double price) // 시장가 매수 
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
        public RequestOrderInfo MakeOrderMarketSell(Param Param, string market, double volume, double price) // 시장가 매도
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