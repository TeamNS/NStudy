using System;
using System.Collections.Generic;
using AccountModels;

namespace AccountData
{
    public class AccountRepo
    {

        public bool RegistAccount(string ID, string PW)
        {
            var AccountList = AccountRepository.Repository.Instance.AccountList;
            return AccountList.TryAdd(ID, new AccountInfo { ID = ID, PW = PW });
        }

        public AccountInfo GetAccountInfo(string ID)
        {
            var AccountList = AccountRepository.Repository.Instance.AccountList;
            if (AccountList.ContainsKey(ID) == false)
            {
                return null;
            }

            return AccountList[ID];
        }

        public bool CheckLogin(string ID, string PW)
        {
            var AccountList = AccountRepository.Repository.Instance.AccountList;
            if (AccountList.ContainsKey(ID) == false)
            {
                return false;
            }

            if (AccountList[ID].PW.CompareTo(PW) != 0)
            {
                return false;
            }

            return true;

        }

        public IEnumerable<AccountInfo> GetAccountList()
        {
            var accountList = new List<AccountInfo>();

            foreach (var item in AccountRepository.Repository.Instance.AccountList)
            {
                accountList.Add(item.Value);
            }

            return accountList;
        }
    }
}