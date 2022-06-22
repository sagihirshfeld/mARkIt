using System;
using Foundation;
using UIKit;
using Xamarin.Auth;
using mARkIt.Authentication;

namespace mARkIt.iOS
{
    public partial class SignInViewController : UIViewController, IAuthenticationDelegate
    {
        private bool m_HasLoggedIn = false;

        public SignInViewController(IntPtr handle) : base(handle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            FacebookButton.TouchUpInside += FacebookButton_TouchUpInside;
            GoogleButton.TouchUpInside += GoogleButton_TouchUpInside;
            NavigationItem.SetHidesBackButton(true, false);
        }

        private void GoogleButton_TouchUpInside(object sender, EventArgs e)
        {
            OAuth2Authenticator oauth2authenticator = LoginHelper.GetGoogle2Authenticator(this);
            PresentViewController(oauth2authenticator.GetUI(), true, null);
        }

        private void FacebookButton_TouchUpInside(object sender, EventArgs e)
        {
            OAuth2Authenticator oauth2authenticator = LoginHelper.GetFacebook2Authenticator(this);
            PresentViewController(oauth2authenticator.GetUI(), true, null);
        }


        public override bool ShouldPerformSegue(string segueIdentifier, NSObject sender)
        {
            if (segueIdentifier == "launchAppSegue") //after press login moveOnlyIfLoggedIn 
            {
                return m_HasLoggedIn;
            }
            return true;
        }


        public async void OnAuthenticationCompleted(Account i_Account)
        {
            DismissViewController(true, null);
            try
            {
                await LoginHelper.SaveAccountAndLoginToBackend(i_Account);
                m_HasLoggedIn = true;
                PerformSegue("launchAppSegue", this);
            }
            catch
            {
               Helpers.Alert.Display("Error", "There was a problem, please try again later.", this);
               SecureStorageAccountStore.RemoveAllAccounts();
            }
        }


        public void OnAuthenticationFailed(string i_Message, Exception i_Exception)
        {
            DismissViewController(true, null);
            SecureStorageAccountStore.RemoveAllAccounts();
            Helpers.Alert.Display("Error", "There was a problem, please try again later.", this); 
        }

        public void OnAuthenticationCanceled()
        {
        }
    }
}
