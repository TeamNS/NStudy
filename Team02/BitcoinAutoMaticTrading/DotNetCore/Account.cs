using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore
{

    public class Account
    {
       // public List<PossessionCoinInfo> m_PossessionCoinInfo = new List<PossessionCoinInfo<>();
        private NoParam noparam { get; set; }
        private Param param { get; set; }
        public Account(string upbitAccessKey, string upbitSecretKey)
        {
            noparam = new NoParam(upbitAccessKey, upbitSecretKey);
            param = new Param(upbitAccessKey, upbitSecretKey);
        }

        public NoParam GetNoParam()
        {
            return noparam;
        }

        public Param GetParam()
        {
            return param;
        }
    }
}
