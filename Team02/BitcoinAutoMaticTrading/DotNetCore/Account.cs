using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore
{

    public class Account
    {
       // private List<PossessionCoinInfo> m_PossessionCoinInfo { get; set; } = new List<PossessionCoinInfo>();
        private NoParam noparam { get; set; }
        private Param param { get; set; }

        private string m_AccessKey { get; set; }

        private string m_SecretKey { get; set; }

        public Account()
        {
        }

        public Account(string upbitAccessKey, string upbitSecretKey)
        {
            m_AccessKey = new string(upbitAccessKey);
            m_SecretKey = new string(upbitSecretKey);

            noparam = new NoParam(m_AccessKey, m_SecretKey);
            param = new Param(m_AccessKey, m_SecretKey);
        }

        public NoParam GetNoParam()
        {
            return noparam;
        }

        public Param GetParam()
        {
            return param;
        }

        public string GetAccessKey()
        {
            return m_AccessKey;
        }
        public string GetSecretKey()
        {
            return m_SecretKey;
        }

        //public List<PossessionCoinInfo> GetPossessionCoinInfo()
        //{
        //    return m_PossessionCoinInfo;
        //}
    }
}
