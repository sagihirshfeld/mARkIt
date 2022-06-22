using mARkIt.Authentication;
using mARkIt.iOS.CoreServices;
using mARkIt.iOS.Helpers;
using mARkIt.iOS.Notifications;
using System;
using UIKit;
using WikitudeComponent.iOS;
using Xamarin.Auth;

namespace mARkIt.iOS
{
    public partial class InitViewController : UIViewController
    {
        private WTAuthorizationRequestManager m_AuthorizationRequestManager = new WTAuthorizationRequestManager();

        public InitViewController(IntPtr handle) : base(handle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            getWikitudePermissions();
        }

        private void getWikitudePermissions()
        {
            WTFeatures requiredFeatures = WTFeatures.Geo | WTFeatures.WTFeature_InstantTracking;

            ArExperienceAuthorizationController.AuthorizeRestricedAPIAccess(m_AuthorizationRequestManager, requiredFeatures, () =>
            {
                autoConnect();
            }, (UIAlertController alertController) =>
            {
                Alert.Display("Permissions Denied", "You cannot proceed without granting permissions", this, (r) => Environment.Exit(0));
            });
        }

        private async void autoConnect()
        {
            try
            {
                await LoginHelper.AutoConnect();
            }
            catch
            {

            }

            if (App.ConnectedUser == null)
            {
                PerformSegue("loginSegue", this);
            }
            else
            {
                PerformSegue("launchAppSegue", this);
            }

        }
    }
}