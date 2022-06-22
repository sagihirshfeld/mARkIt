using mARkIt.Authentication;
using mARkIt.iOS.Notifications;
using mARkIt.Notifications;
using System;
using UIKit;

namespace mARkIt.iOS
{
    public partial class TabBarController : UITabBarController
    {
        public TabBarController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            MarksScanner marksScanner = IOSMarksScanner.Instance;
            marksScanner.StartScanning();
            LoginHelper.LoggedOut += marksScanner.StopScanning;
        }
    }
}
