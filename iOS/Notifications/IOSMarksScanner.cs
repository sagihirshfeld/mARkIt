using System;
using CoreLocation;
using Foundation;
using mARkIt.Notifications;

namespace mARkIt.iOS.Notifications
{
    /// <remarks>
    ///     - Since iOS is very restrictive of most continues long-running background tasks,
    ///       this implementation uses the iOS-approved location-updates task to keep polling the backend consistently.
    /// </remarks>
    public sealed class IOSMarksScanner : MarksScanner
    {
        private static IOSMarksScanner s_Instance = null;
        private static object s_LockObj = new Object();

        private CLLocationManager m_LocationManager;
        private DateTime m_LastScanTime;

        public static IOSMarksScanner Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    lock (s_LockObj)
                    {
                        if (s_Instance == null)
                        {
                            s_Instance = new IOSMarksScanner();
                        }
                    }
                }

                return s_Instance;
            }
        }

        private IOSMarksScanner()
        {
            m_LocationManager = new CLLocationManager();
            m_LocationManager.AllowsBackgroundLocationUpdates = true;
            m_LocationManager.PausesLocationUpdatesAutomatically = false;
            m_LocationManager.DesiredAccuracy = 1;
            m_LocationManager.Failed += locMgr_OnFailure;
            m_LocationManager.LocationsUpdated += locMgr_OnLocationsUpdated;
        }

        public override void StartScanning()
        {
            if (CLLocationManager.LocationServicesEnabled)
            {
                m_LastScanTime = DateTime.Now;

                m_LocationManager.StartUpdatingLocation();
            }
        }

        public override void StopScanning()
        {
            m_LocationManager.StopUpdatingLocation();
        }

        private async void locMgr_OnLocationsUpdated(object sender, CLLocationsUpdatedEventArgs e)
        {
            CLLocation lastKnownPosition = e.Locations[e.Locations.Length - 1];
            TimeSpan timeSinceLastScan = DateTime.Now - m_LastScanTime;

            if (timeSinceLastScan.Minutes >= 1)
            {
                m_LastScanTime = DateTime.Now;

                base.CheckForClosestNewMark(lastKnownPosition.Coordinate.Latitude, lastKnownPosition.Coordinate.Longitude);
            }
        }

        private void locMgr_OnFailure(object sender, NSErrorEventArgs e)
        {
            Console.WriteLine("didFailWithError " + e.Error);
            Console.WriteLine("didFailWithError coe " + e.Error.Code);
        }

        protected override ILocalNotification CreateLocalNotification()
        {
            return new IOSLocalNotification();
        }

        protected override void LogScanningException(Exception e)
        {
            Console.WriteLine("ClosestMarkScanFail: " + e.Message);
        }
    }
}