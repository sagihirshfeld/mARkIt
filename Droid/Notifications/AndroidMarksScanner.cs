using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using mARkIt.Services;
using System;
using System.Threading.Tasks;
using Plugin.Geolocator.Abstractions;
using mARkIt.Models;
using System.Net.Http;
using System.Collections.Generic;
using Plugin.Geolocator;
using mARkIt.Notifications;
using System.Threading;
using System.Timers;

namespace mARkIt.Droid.Notifications
{
    public sealed class AndroidMarksScanner : MarksScanner
    {
        private static AndroidMarksScanner s_Instance = null;
        private static object s_LockObj = new Object();

        private System.Timers.Timer m_ScanTimer;
        private PowerManager.WakeLock m_WakeLock;
        private Context m_Context;

        private AndroidMarksScanner() { }

        public static AndroidMarksScanner GetInstance(Context context)
        {
            if (s_Instance == null)
            {
                lock (s_LockObj)
                {
                    if (s_Instance == null)
                    {
                        s_Instance = new AndroidMarksScanner(context);
                    }
                }
            }

            else
            {
                s_Instance.m_Context = context;
            }

            return s_Instance;
        }

        private AndroidMarksScanner(Context context)
        {
           m_Context = context;
        }

        public override void StartScanning()
        {
            // Prevent the Android OS from killing our process while it is in the background
            PowerManager pw = (PowerManager)m_Context.GetSystemService(Context.PowerService);
            m_WakeLock = pw.NewWakeLock(WakeLockFlags.Full, "MarkitWakelock");
            m_WakeLock.Acquire();

            // Poll the web server every minute for new marks to notify about
            Task.Run(() =>
            {
                m_ScanTimer = new System.Timers.Timer();
                m_ScanTimer.Interval = TimeSpan.FromSeconds(60).TotalMilliseconds;
                m_ScanTimer.Enabled = true;
                m_ScanTimer.Elapsed += ScanTimer_OnElapsed;
                m_ScanTimer.Start();
            });
        }

        public override void StopScanning()
        {
            if (m_ScanTimer != null && m_WakeLock != null)
            {
                m_ScanTimer.Stop();
                m_WakeLock.Release();
            }
        }

        private async void ScanTimer_OnElapsed(object sender, ElapsedEventArgs e)
        {
            Position lastKnownPosition = await CrossGeolocator.Current.GetLastKnownLocationAsync();
            base.CheckForClosestNewMark(lastKnownPosition.Latitude, lastKnownPosition.Longitude);
        }

        protected override ILocalNotification CreateLocalNotification()
        {
            return new AndroidLocalNotification(m_Context);
        }

        protected override void LogScanningException(Exception e)
        {
            Android.Util.Log.Debug("Markit", e.Message);

        }
    }
}
