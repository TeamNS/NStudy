using System;
using System.Collections.Generic;
using AccountModels;


//todo db로 변경
namespace AccountRepository
{
    public sealed class Repository
    {
        public Dictionary<string, AccountInfo> AccountList = new Dictionary<string, AccountInfo>(); 

        //private 생성자 
        private Repository() { }

        private static readonly Lazy<Repository> _instance = new Lazy<Repository> (() => new Repository());

        public static Repository Instance { get { return _instance.Value; } }
    }
}