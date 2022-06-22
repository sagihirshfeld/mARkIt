using mARkIt.Models;
using mARkIt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace mARkIt.Notifications
{
    /// <summary>
    ///     Scans and notifies of any new marks around the user, using GPS and local notifications.
    /// </summary>
    public abstract class MarksScanner
    {
        public abstract void StartScanning();

        public abstract void StopScanning();

        protected async void CheckForClosestNewMark(double currentLatitude, double currentLongitude)
        {
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>
                    {
                        {"latitude", currentLatitude.ToString() },
                        {"longitude", currentLongitude.ToString() }
                    };

                Mark closestMark = await AzureWebApi.MobileService.InvokeApiAsync<Mark>("ClosestMark", HttpMethod.Get, parameters);

                if (closestMark != null)
                {
                    ILocalNotification localNotification = CreateLocalNotification();
                    localNotification.Show("mARkIt", "A new mark is closeby!");
                }
            }

            catch (Exception e)
            {
                LogScanningException(e);
            }
        }

        protected abstract ILocalNotification CreateLocalNotification();

        protected abstract void LogScanningException(Exception e);
    }
}
