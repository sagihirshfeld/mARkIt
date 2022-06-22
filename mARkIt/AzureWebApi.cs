using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using mARkIt.Abstractions;
using mARkIt.Authentication;
using mARkIt.Models;
using mARkIt.Utils;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using Xamarin.Auth;

namespace mARkIt.Services
{
    public class AzureWebApi
    {
        private static string BackendURL = "https://mark-api.azurewebsites.net/";

        public static MobileServiceClient MobileService = new MobileServiceClient(BackendURL);

        public static bool IsConnected
        {
            get
            {
                return MobileService.CurrentUser != null;
            }
        }

        public static async Task Login(MobileServiceAuthenticationProvider i_AuthType, Account i_Account)
        {
            var zumoPayload = makeZumoPayload(i_AuthType, i_Account);

            try
            {
                await MobileService.LoginAsync(i_AuthType, zumoPayload);
            }

            catch (MobileServiceInvalidOperationException e)
            {
                if (e.Response.ReasonPhrase == "Unauthorized")
                {
                    throw new UnauthorizedException(i_AuthType);
                }

                else
                {
                    throw e;
                }
            }

            App.ConnectedUser = await getUserAsync();
        }

        private async static Task<User> getUserAsync()
        {
            string loggedUserId = getLoggedUserId();

            User user = await GetById<User>(loggedUserId);

            if (user == null)
            {
                // Add the user to the database
                user = new User()
                {
                    Id = loggedUserId,
                    relevantCategoriesCode = (int)eCategories.All
                };

                await Insert(user);
                user = await GetById<User>(loggedUserId);
            }

            return user;
        }

        private static string getLoggedUserId()
        {
            string loggedUserId = string.Empty;

            if (MobileService.CurrentUser != null)
            {
                // Azure places "sid:" at the start of the generated id, which we want to remove to avoid redundancy and URI errors (because of ':' character)
                loggedUserId = MobileService.CurrentUser.UserId.Replace("sid:", "");
            }

            return loggedUserId;
        }

        private static JObject makeZumoPayload(MobileServiceAuthenticationProvider i_AuthType, Account i_Account)
        {
            var zumoPayload = new JObject();
            zumoPayload.Add("access_token", i_Account.Properties["access_token"]);

            if (i_AuthType == MobileServiceAuthenticationProvider.Google)
            {
                zumoPayload.Add("id_token", i_Account.Properties["id_token"]);
            }

            return zumoPayload;
        }

        public static async Task<bool> Insert<T>(T i_ObjectToInsert)
        {
            try
            {
                await MobileService.GetTable<T>().InsertAsync(i_ObjectToInsert);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<bool> Delete<T>(T i_ObjectToDelete)
        {
            try
            {
                await MobileService.GetTable<T>().DeleteAsync(i_ObjectToDelete);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<T> GetById<T>(string i_Id) where T : TableData
        {
            T res = null;

            try
            {
                res = await MobileService.GetTable<T>().LookupAsync(i_Id);
            }

            catch (Exception e)
            {
                return null;
            }

            return res;
        }

        public static async Task<bool> Update<T>(T i_ObjectToUpdate)
        {
            try
            {
                await MobileService.GetTable<T>().UpdateAsync(i_ObjectToUpdate);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task Logout()
        {
            await MobileService.LogoutAsync();
            App.ConnectedUser = null;
        }
    }
}