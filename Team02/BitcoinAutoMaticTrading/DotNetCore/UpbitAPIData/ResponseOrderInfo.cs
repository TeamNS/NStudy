using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore
{
    public class ResponseOrderInfo
    {
        public string uuid;                 // 주문의 고유 아이디
        public string side;                 // 주문 종류
        public string ord_type;             // 주문 방식
        public double price;                // 주문 당시 화폐 가격
        public double avg_price;            // 체결 가격의 평균가
        public string state;                // 주문 상태
        public string market;               // 마켓의 유일키
        public string created_at;           // 주문 생성 시간
        public double volume;               // 사용자가 입력한 주문양
        public double remaining_volume;     // 체결 후 남은 주문 양
        public double reserved_fee;         // 수수료로 예약된 비용
        public double remaining_fee;        // 남은 수수료
        public double paid_fee;             // 사용된 수수료
        public double locked;               // 거래에 사용중인 비용
        public double executed_volume;      // 체결된 양
        public int trade_count;             // 해당 주문에 걸링 체결 수 
    }
}
