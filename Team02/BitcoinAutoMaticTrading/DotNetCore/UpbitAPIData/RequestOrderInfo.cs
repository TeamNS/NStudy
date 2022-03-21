﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore
{
    public class RequestOrderInfo
    {
        public string market;       // 마켓 ID
        public string side;         // bid : 매수, ask : 매도
        public double volume;
        public double price;        // 주문 가격. (지정가, 시장가 매수 시 필수)
                                    // ex) KRW-BTC 마켓에서 1BTC당 1,000 KRW로 거래할 경우, 값은 1000 이 된다.
                                    // ex) KRW-BTC 마켓에서 1BTC당 매도 1호가가 500 KRW 인 경우, 시장가 매수 시 값을 1000으로 세팅하면 2BTC가 매수된다.
        public string ord_type;     // 주문 타입 (필수)
                                    // limit : 지정가 주문
                                    // price : 시장가 주문(매수)
                                    // market : 시장가 주문(매도)
        public string identifier;   // 조회용 사용자 지정값(선택)

    }
}
