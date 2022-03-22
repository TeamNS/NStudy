using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore
{
    public class PossessionCoinInfo : IDisposable
    {
        public string currency;
        public double balance;
        public double locked;
        public double avg_buy_price;
        public bool avg_buy_price_modified;
        public string unit_currency;
        public bool bAutoTrading { get; set; } = false;

        public void Dispose() { } // using{} 키워드를 사용하기 위함 

    }
}