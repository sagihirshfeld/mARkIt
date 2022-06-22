using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Essentials;

namespace mARkIt.Authentication
{
    public static class SecureStorageAccountStore
    {
        // store the account based on the given service type
        public static async Task SaveAccountAsync(Account i_Account, string i_ServiceType)
        {
            // remove an already existing account of this service type if exists
            SecureStorage.Remove(i_ServiceType);

            // convert the account to json format
            // we will put it in a list so it will be convertiable
            List<Account> listOfAccounts = new List<Account>();
            listOfAccounts.Add(i_Account);
            string listOfAccountsInJsonFormat = JsonConvert.SerializeObject(listOfAccounts);

            // Securely save the accounts for the given service
            await SecureStorage.SetAsync(i_ServiceType, listOfAccountsInJsonFormat);
        }

        // return a stored account based on the given service type
        // returns null if there is no account stored for this service
        public static async Task<Account> GetAccountAsync(string i_ServiceType)
        {
            string accountInJsonFormat = await SecureStorage.GetAsync(i_ServiceType);
            List<Account> listOfAccounts = null;
            Account accountToReturn = null;
            // try to deserialize the json in Account object
            try
            {
                listOfAccounts = JsonConvert.DeserializeObject<List<Account>>(accountInJsonFormat);
            }
            catch (Exception)
            {
                listOfAccounts = null;
            }
            
            if (listOfAccounts != null && listOfAccounts.Count > 0)
            {
                accountToReturn = listOfAccounts[0];
            }
            return accountToReturn;
        }

        // remove all accounts that are currently stored
        public static void RemoveAllAccounts()
        {
            SecureStorage.RemoveAll();
        }
    }
}