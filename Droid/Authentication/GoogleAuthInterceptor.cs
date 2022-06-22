using Android.App;
using Android.Content;
using Android.OS;
using mARkIt.Authentication;
using System;

namespace mARkIt.Droid.Authentication
{
    [Activity(Label = "GoogleAuthInterceptor")]
    [
        IntentFilter
        (
            actions: new[] { Intent.ActionView },
            Categories = new[]
            {
                Intent.CategoryDefault,
                Intent.CategoryBrowsable
            },
            DataSchemes = new[]
            {
                mARkIt.Utils.Configuration.PackageName
            },
            DataPaths = new[]
            {
                mARkIt.Utils.Configuration.RedirectUrl
            }
        )
    ]
    public class GoogleAuthInterceptor : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Android.Net.Uri uri_android = Intent.Data;
            Uri uri_netfx = new Uri(uri_android.ToString());
            LoginHelper.s_GoogleAuthenticator?.OnPageLoading(uri_netfx);
            Finish();
        }
    }
}