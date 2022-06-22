using mARkIt.Models;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace mARkIt.Authentication
{
    public class AuthenticatorUserDataProvider
    {
        private string requestUri;
        private string facebookRequestUri;
        protected HttpClient m_HttpClient;

        public AuthenticatorUserDataProvider(Account i_Account, MobileServiceAuthenticationProvider i_AuthType)
        {
            if (i_Account != null)
            {
                m_HttpClient = new HttpClient();
                string accessToken = i_Account.Properties["access_token"];

                if(i_AuthType == MobileServiceAuthenticationProvider.Facebook)
                {
                    requestUri = $"https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token={accessToken}";
                }
                else
                {
                    requestUri = $"https://graph.facebook.com/me?fields=first_name,last_name,email&access_token={accessToken}";
                }
            }
        }

        public async Task<User> GetUserAsync()
        {
            string jsonResponse = await m_HttpClient.GetStringAsync(requestUri);
            User user = JsonConvert.DeserializeObject<User>(jsonResponse);
            return user;
        }
    }
}