using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore
{
    public class AccountInformation
    {
        public int AccountUID { get; set; }
        public string PID { get; set; }
        public string Name { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
    }
}
