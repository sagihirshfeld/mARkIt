using Android.Content;
using Android.Support.V7.App;
using mARkIt.Authentication;
using mARkIt.Droid.Notifications;
using mARkIt.Notifications;

namespace mARkIt.Droid.Activities
{
    public abstract class PriorToMainAppActivity : AppCompatActivity
    {
        protected void StartMainApp()
        {
            MarksScanner marksScanner = AndroidMarksScanner.GetInstance(context: this);
            marksScanner.StartScanning();
            LoginHelper.LoggedOut += marksScanner.StopScanning;

            Intent mainTabs = new Intent(this, typeof(TabsActivity));
            mainTabs.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            StartActivity(mainTabs);
            Finish();
        }
    }
}