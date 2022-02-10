using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Text;
using Newtonsoft.Json;

using System.Threading;



namespace BitcoinAutoMaticTrading
{
    public class UpbitAPI
    {
        const int INPUT_COUNT = 5;

        private NoParam noparam;
        private Param param;

        //private List<Account> ListAccount;
        private List<MarketInfo> m_ListMarketInfo;
        private List<List<CandleMinute>> m_ListCandleMinutes = new List<List<CandleMinute>>();
        Dictionary<String, List<CandleMinute>> m_DicCoinCandleMinutes = new Dictionary<String, List<CandleMinute>>();

        public UpbitAPI(string upbitAccessKey, string upbitSecretKey)
        {
            noparam = new NoParam(upbitAccessKey, upbitSecretKey);
            param = new Param(upbitAccessKey, upbitSecretKey);
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
                if (Str.Substring(0, 3) != "KRW") // 원화 마켓에 있는 데이터만 긁어옴
                    continue;

                if (m_DicCoinCandleMinutes.Count >= INPUT_COUNT)
                    break;

                m_DicCoinCandleMinutes.Add(m_ListMarketInfo[i].market, api.GetCandleMinutes(m_ListMarketInfo[i].market, UpbitAPI.MinuteUnit._1, DateTime.Now, 1));
            }
        }
        public void print(List<Account> ListAccount)
        {
            for (int i = 0; i < ListAccount.Count; ++i) // 해당 계좌가 가지고 있는 코인 정보
                Console.WriteLine(ListAccount[i].currency + " | " + ListAccount[i].avg_buy_price + " | " + ListAccount[i].unit_currency);

            Console.WriteLine();

            foreach (KeyValuePair<String, List<CandleMinute>> CandleInfo in m_DicCoinCandleMinutes)
            {
                Console.WriteLine("코인 :" + CandleInfo.Key + " | 현재 거래 가격 : " + CandleInfo.Value[0].trade_price + " | 거래량 : " + CandleInfo.Value[0].candle_acc_trade_volume + " | 현재 시간 : " + CandleInfo.Value[0].candle_date_time_kst);
            }

            Console.WriteLine();
            m_DicCoinCandleMinutes.Clear();
            //Console.Clear();
            // }
            // Console.ReadLine();
        }


        //public void Update()
        //{
        //    // 추후엔 버튼을 누르는 등의 input이 있을떄 동작
        //
        //    while ()
        //    {
        //        for (int i = 0; i < ListAccount.Count; ++i)
        //            Console.WriteLine(ListAccount[i].currency);
        //
        //        Console.WriteLine();
        //
        //        for (int i = 0; i < ListMarketInfo.Count; ++i)
        //            Console.WriteLine(ListMarketInfo[i].korean_name + "-" + ListMarketInfo[i].market + "-");
        //
        //        Console.WriteLine();
        //
        //        for (int i = 0; i < ListCandleMinutes.Count; ++i)
        //            Console.WriteLine(ListCandleMinutes[i].market + " | " + ListCandleMinutes[i].trade_price + " | " + ListCandleMinutes[i].candle_date_time_kst);
        //
        //        Console.ReadLine();
        //    }
        //}

        public List<Account> GetAccount()
        {
            var data = noparam.Get("/v1/accounts", RestSharp.Method.Get);
            return JsonConvert.DeserializeObject<List<Account>>(data); // 하나씩 하려고 하니까 뻑남, JsonConvert.DeserializeObject 확인 해볼 필요 있음
        }

        public List<MarketInfo> GetMarketInfo()
        {
            var data = noparam.Get("/v1/market/all", RestSharp.Method.Get);
            return JsonConvert.DeserializeObject<List<MarketInfo>>(data);
        }

        public List<CandleMinute> GetCandleMinutes(string market, MinuteUnit unit, DateTime to = default(DateTime), int count = 1)
        {
            // 시세 캔들 조회 - 분(Minute) 캔들
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("market", market);
            parameters.Add("to", to.ToString("yyyy-MM-dd HH:mm:ss"));
            parameters.Add("count", count.ToString());
            var data = param.Get(String.Join("", "/v1/candles/minutes/", (int)unit), parameters, RestSharp.Method.Get);
            return JsonConvert.DeserializeObject<List<CandleMinute>>(data);
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