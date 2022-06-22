using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System;
using Xamarin.Auth;
using mARkIt.Authentication;

namespace mARkIt.Droid.Activities
{
    [Activity(Label = "Login")]
    public class LoginActivity: PriorToMainAppActivity , IAuthenticationDelegate
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            SetContentView(Resource.Layout.Login);
            
            ImageButton facebookLoginButton = FindViewById<ImageButton>(Resource.Id.facebook_login_button);
            facebookLoginButton.Click += OnFacebookLoginButtonClicked;
            
            ImageButton googleLoginButton = FindViewById<ImageButton>(Resource.Id.google_login_button);
            googleLoginButton.Click += OnGoogleLoginButtonClicked;
        }

        private void OnFacebookLoginButtonClicked(object sender, EventArgs e)
        {
            var oauth2authenticator = LoginHelper.GetFacebook2Authenticator(this);
            Intent facebookIntent = oauth2authenticator.GetUI(this);
            StartActivity(facebookIntent);
        }

        private void OnGoogleLoginButtonClicked(object sender, EventArgs e)
        {
            OAuth2Authenticator oauth2authenticator = LoginHelper.GetGoogle2Authenticator(this);
            Intent googleIntent = oauth2authenticator.GetUI(this);
            CustomTabsConfiguration.CustomTabsClosingMessage = null;
            StartActivity(googleIntent);
        }

        public async void OnAuthenticationCompleted(Account i_Account)
        {
            try
            {
                await LoginHelper.SaveAccountAndLoginToBackend(i_Account);
                StartMainApp();
            }
            catch
            {
                Helpers.Alert.Show("Error", "There was a problem, please try again later.", this);
                SecureStorageAccountStore.RemoveAllAccounts();
            }
        }

        public void OnAuthenticationFailed(string i_Message, Exception i_Exception)
        {
            // if permission was not granted, we do not want to store any account - to be safe
            Helpers.Alert.Show("Error", "There was a problem, please try again later.", this);
            SecureStorageAccountStore.RemoveAllAccounts();
        }

        public void OnAuthenticationCanceled()
        {
            // user canceled (pressed back) during auth process
            Helpers.Alert.Show("Authentication canceled", "You didn't complete the authentication process", this);
        }
    }
}

