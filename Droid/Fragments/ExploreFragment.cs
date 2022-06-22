using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Locations;
using Android.Webkit;
using Com.Wikitude.Architect;
using Com.Wikitude.Common.Camera;
using Org.Json;
using mARkIt.Droid.Activities;
using mARkIt.Models;
using Newtonsoft.Json;

namespace mARkIt.Droid.Fragments
{
    public class ExploreFragment : Android.Support.V4.App.Fragment, ILocationListener, ArchitectView.ISensorAccuracyChangeListener,IArchitectJavaScriptInterfaceListener
    {
        public readonly static string IntentExtrasKeyExperienceData = "ExperienceData";
        private Location.LocationProvider locationProvider;

        protected ArchitectView architectView;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            locationProvider = new Location.LocationProvider(Context, this);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            WebView.SetWebContentsDebuggingEnabled(true);

            architectView = new ArchitectView(Context);
            return architectView;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            var arExperiencePath = "ARPages/ARPage/index.html";

            var config = new ArchitectStartupConfiguration
            {
                LicenseKey = Utils.Keys.WikitudeLicense,
                CameraPosition = CameraSettings.CameraPosition.Back,
                CameraResolution = CameraSettings.CameraResolution.FULLHD1920x1080,
                CameraFocusMode = CameraSettings.CameraFocusMode.Continuous,
                ArFeatures = ArchitectStartupConfiguration.Features.ImageTracking | ArchitectStartupConfiguration.Features.Geo 
            };

            architectView.OnCreate(config);
            architectView.OnPostCreate();
            architectView.Load(arExperiencePath);
            architectView.AddArchitectJavaScriptInterfaceListener(this);
        }

        public override void OnResume()
        {
            base.OnResume();
            architectView.OnResume();
            if (!locationProvider.Start())
            {
                Toast.MakeText(Context, "Could not start Location updates. Make sure that locations and location providers are enabled and Runtime Permissions are granted.", ToastLength.Long).Show();
            }
            architectView.RegisterSensorAccuracyChangeListener(this);
        }

        public override void OnPause()
        {
            base.OnPause();
            architectView.OnPause();
            locationProvider.Stop();
            architectView.UnregisterSensorAccuracyChangeListener(this);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            architectView.ClearCache();
            architectView.OnDestroy();
        }

        public void OnLocationChanged(Android.Locations.Location location)
        {
            float accuracy = location.HasAccuracy ? location.Accuracy : 1000;
            if (location.HasAltitude)
            {
                architectView.SetLocation(location.Latitude, location.Longitude, location.Altitude, accuracy);
            }
            else
            {
                architectView.SetLocation(location.Latitude, location.Longitude, accuracy);
            }
        }

        public void OnProviderDisabled(string provider) { }

        public void OnProviderEnabled(string provider) { }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras) { }

        public void OnCompassAccuracyChanged(int accuracy)
        {
            
        }

        public void OnJSONObjectReceived(JSONObject p0)
        {
            string option = p0.GetString("option");
            Intent intent;
            if (option == "add")
            {
                intent = new Intent(Activity, typeof(AddAMarkActivity));
                StartActivity(intent);
            }
            else if (option == "rate") 
            {
                intent = new Intent(Activity, typeof(RateAMarkActivity));
                intent.PutExtra("markId", p0.GetString("markId"));
                StartActivity(intent);
            }
            else if (option == "getMarks")
            {
                double longitude = p0.GetDouble("longitude");
                double latitude = p0.GetDouble("latitude");
                getMarks(longitude, latitude);
            }
            else if (option == "seen")
            {
                User.UpdateMarkSeen(p0.GetString("markId"));
            }
        }


        private async void getMarks(double i_Longitude, double i_Latitude)
        {
            var list = await Mark.GetRelevantMarks(i_Longitude, i_Latitude, 1.0);
            if (list != null)
            {
                var jsonList = JsonConvert.SerializeObject(list);
                string functionCall = @"setMarks(" + jsonList + ")";
                architectView.CallJavascript(functionCall);
            }
        }
    }
}
