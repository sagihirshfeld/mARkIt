using mARkIt.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace mARkIt.Authentication
{
    public class FacebookAuthenticator
    {
        public const string FacebookAuth = "https://m.facebook.com/dialog/oauth/";
        private const bool IsUsingNativeUI = false;
        private OAuth2Authenticator m_OAuth2Authenticator;
        private IAuthenticationDelegate m_AuthenticationDelegate;

        public FacebookAuthenticator(string i_ClientId,
                                     string i_Scope,
                                     IAuthenticationDelegate i_AuthenticationDelegate)
        {
            m_OAuth2Authenticator = new OAuth2Authenticator(
                                        i_ClientId,
                                        i_Scope,
                                        new Uri(FacebookAuth),
                                        new Uri(Configuration.FacebookRedirectUrl),
                                        null,
                                        IsUsingNativeUI);
            m_AuthenticationDelegate = i_AuthenticationDelegate;
            m_OAuth2Authenticator.Completed += OnAuthenticationCompleted;
            m_OAuth2Authenticator.Error += OnAuthenticationFailed;
        }

        public OAuth2Authenticator GetOAuth2()
        {
            return m_OAuth2Authenticator;
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
