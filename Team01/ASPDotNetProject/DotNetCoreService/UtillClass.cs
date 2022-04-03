using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreService
{
    public class UtillClass
    {
        private static Dictionary<int, string> m_DicToken;

        static UtillClass()
        {
            m_DicToken = new Dictionary<int, string>();
        }

        public static void Add()
        {
        }

        public static void Print()
        {
            foreach(var d in m_DicToken)
            {
                Console.WriteLine(d);
            }
        }
    }
}
