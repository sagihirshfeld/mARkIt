using mARkIt.Utils;
using System;
using Xamarin.Auth;

namespace mARkIt.Authentication
{
    public class GoogleAuthenticator
    {
        public const string GoogleAuth = "https://accounts.google.com/o/oauth2/v2/auth";
        public const string GoogleAccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
        private const bool IsUsingNativeUI = true;
        private OAuth2Authenticator m_OAuth2Authenticator;
        private IAuthenticationDelegate m_AuthenticationDelegate;

        public GoogleAuthenticator(string i_ClientId,
                                     string i_Scope,
                                     IAuthenticationDelegate i_AuthenticationDelegate)
        {
            m_OAuth2Authenticator = new OAuth2Authenticator(
                                        i_ClientId,
                                        string.Empty,
                                        i_Scope,
                                        new Uri(GoogleAuth),
                                        new Uri(Configuration.GoogleRedirectUrl),
                                        new Uri(GoogleAccessTokenUrl),
                                        null,
                                        IsUsingNativeUI);
            m_AuthenticationDelegate = i_AuthenticationDelegate;
            m_OAuth2Authenticator.Completed += OnAuthenticationCompleted;
            m_OAuth2Authenticator.Error += OnAuthenticationFailed;
        }

        public GoogleAuthenticator(string i_ClientId, string i_Scope)
        {
            m_OAuth2Authenticator = new OAuth2Authenticator(
                            i_ClientId,
                            string.Empty,
                            i_Scope,
                            new Uri(GoogleAuth),
                            new Uri(Configuration.GoogleRedirectUrl),
                            new Uri(GoogleAccessTokenUrl),
                            null,
                            IsUsingNativeUI);
        }

        public OAuth2Authenticator GetOAuth2()
        {
            return m_OAuth2Authenticator;
        }

        public void OnPageLoading(Uri i_Uri)
        {
            m_OAuth2Authenticator.OnPageLoading(i_Uri);
        }

        private void OnAuthenticationCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                m_AuthenticationDelegate.OnAuthenticationCompleted(e.Account);
            }
            else
            {
                m_AuthenticationDelegate.OnAuthenticationCanceled();
            }
        }

        private void OnAuthenticationFailed(object sender, AuthenticatorErrorEventArgs e)
        {
            m_AuthenticationDelegate.OnAuthenticationFailed(e.Message, e.Exception);
        }
    }
}
