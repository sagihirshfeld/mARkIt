using System;
using System.Threading.Tasks;
using mARkIt.Services;
using mARkIt.Utils;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Auth;

namespace mARkIt.Authentication
{
    public class LoginHelper
    {
        public static event Action LoggedIn;
        public static event Action LoggedOut;

        private static Account s_Account;
        public static GoogleAuthenticator s_GoogleAuthenticator;
        private static FacebookAuthenticator s_FacebookAuthenticator;
        private static MobileServiceAuthenticationProvider s_AuthType;

        public static async Task AutoConnect()
        {
            s_Account = await SecureStorageAccountStore.GetAccountAsync("Facebook");
            s_AuthType = MobileServiceAuthenticationProvider.Facebook;
            if (s_Account == null)
            {
                s_Account = await SecureStorageAccountStore.GetAccountAsync("Google");
                s_AuthType = MobileServiceAuthenticationProvider.Google;
            }

            if (s_Account != null)
            {
                await loginToBackend();
                LoggedIn?.Invoke();
            }
        }

        public static async Task SaveAccountAndLoginToBackend(Account i_Account)
        {
            s_Account = i_Account;

            await saveAccountToDevice(i_Account);
            await loginToBackend();
        }

        public static OAuth2Authenticator GetFacebook2Authenticator(IAuthenticationDelegate i_AuthenticationDelegate)
        {
            s_FacebookAuthenticator = new FacebookAuthenticator(Keys.FacebookAppId, Configuration.FacebookAuthScope, i_AuthenticationDelegate);
            s_AuthType = MobileServiceAuthenticationProvider.Facebook;
            return s_FacebookAuthenticator.GetOAuth2();
        }

        public static OAuth2Authenticator GetGoogle2Authenticator(IAuthenticationDelegate i_AuthenticationDelegate)
        {
            s_GoogleAuthenticator = new GoogleAuthenticator(Keys.GoogleClientId, Configuration.GoogleAuthScope, i_AuthenticationDelegate);
            s_AuthType = MobileServiceAuthenticationProvider.Google;
            return s_GoogleAuthenticator.GetOAuth2();
        }

        public static async Task<bool> Logout()
        {
            try
            {
                await AzureWebApi.Logout();
                Xamarin.Essentials.SecureStorage.RemoveAll();
                LoggedOut?.Invoke();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static async Task loginToBackend()
        {
            try
            {
                await AzureWebApi.Login(s_AuthType, s_Account);
            }

            catch (UnauthorizedException)
            {
                try
                {
                    await refreshToken();
                    await AzureWebApi.Login(s_AuthType, s_Account);
                }

                catch (Exception e)
                {

                }
            }
        }

        private async static Task refreshToken()
        {
            if (s_AuthType == MobileServiceAuthenticationProvider.Google)
            {
                await refreshGoogleTokenAsync();
            }

            else
            {
                refreshFacebookToken();
            }
        }

        private static async Task refreshGoogleTokenAsync()
        {
            GoogleAuthenticator glAuth = new GoogleAuthenticator(Keys.GoogleClientId, Configuration.GoogleAuthScope);
            OAuth2Authenticator oauth2 = glAuth.GetOAuth2();
            oauth2.Completed += OnAuthenticationCompleted_RefreshedToken;
            int refreshTokenExpireTime = await oauth2.RequestRefreshTokenAsync(s_Account.Properties["refresh_token"]);
        }

        private async static void OnAuthenticationCompleted_RefreshedToken(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                e.Account.Properties["refresh_token"] = s_Account.Properties["refresh_token"];
                s_Account = e.Account;
                await saveAccountToDevice(s_Account);
            }
        }

        private static void refreshFacebookToken()
        {
            throw new NotImplementedException();
        }

        private async static Task saveAccountToDevice(Account i_Account)
        {
            if (s_AuthType == MobileServiceAuthenticationProvider.Facebook)
            {
                await SecureStorageAccountStore.SaveAccountAsync(i_Account, "Facebook");
            }

            else if (s_AuthType == MobileServiceAuthenticationProvider.Google)
            {
                await SecureStorageAccountStore.SaveAccountAsync(i_Account, "Google");
            }
        }
    }
}