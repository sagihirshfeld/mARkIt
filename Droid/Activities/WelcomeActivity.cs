using System;
using System.Linq;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using mARkIt.Authentication;
using Com.Wikitude.Architect;
using Com.Wikitude.Common.Permission;
using mARkIt.Droid.Helpers;

namespace mARkIt.Droid.Activities
{
    [Activity(Label = "mARk-It", MainLauncher = true)]
    public class WelcomeActivity : PriorToMainAppActivity, IPermissionManagerPermissionManagerCallback
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            SetContentView(Resource.Layout.Welcome);
            getWikitudePermissions();
        }

        private void getWikitudePermissions()
        {
            string[] permissions = { Manifest.Permission.Camera,
                                     Manifest.Permission.AccessFineLocation,
                                     Manifest.Permission.ReadExternalStorage,
                                     Manifest.Permission.WriteExternalStorage,
                                     };

            ArchitectView.PermissionManager.CheckPermissions(this, permissions, PermissionManager.WikitudePermissionRequest, this);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            int []results = grantResults.Cast<int>().ToArray();
            ArchitectView.PermissionManager.OnRequestPermissionsResult(requestCode, permissions, results);
            ArchitectView.PermissionManager.CheckPermissions(this, permissions, PermissionManager.WikitudePermissionRequest, this);
        }

        public void PermissionsGranted(int responseCode)
        {
            autoConnect();
        }

        private async void autoConnect()
        {
            try
            {
                await LoginHelper.AutoConnect();
            }
            catch (Exception)
            {

            }
            loadApp();
        }

        private void loadApp()
        {
            if (App.ConnectedUser != null)
            {
                StartMainApp();
            }
            else
            {
                startLoginPage();
            }
        }

        private void startLoginPage()
        {
            Intent loginIntent = new Intent(this, typeof(LoginActivity));
            StartActivity(loginIntent);
            Finish();
        }

        public void PermissionsDenied(string[] deniedPermissions)
        {
            showPermissionsDeniedDialog();
        }

        public void ShowPermissionRationale(int requestCode, string[] permissions)
        {
            showPermissionsDeniedDialog();
        }

        private void showPermissionsDeniedDialog()
        {
            Alert.Show("Permissions Denied", "You cannot proceed without granting permissions", this, Finish);
        }
    }
}