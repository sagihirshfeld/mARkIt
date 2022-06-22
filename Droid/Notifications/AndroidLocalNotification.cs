using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using mARkIt.Droid.Activities;
using mARkIt.Notifications;

namespace mARkIt.Droid.Notifications
{
    public class AndroidLocalNotification : ILocalNotification
    {
        static readonly int NOTIFICATION_ID = 1000;
        static readonly string CHANNEL_ID = "location_notification";
        private Context m_Context;
        private Intent m_ResultIntent;

        public AndroidLocalNotification(Context context)
        {
            m_Context = context;
        }

        public AndroidLocalNotification(Context context, Intent resultIntent) : this(context)
        {
            m_ResultIntent = resultIntent;
        }

        public void Show(string title, string message)
        {
            if (m_ResultIntent == null)
            {
                m_ResultIntent = new Intent(m_Context, typeof(TabsActivity));
            }

            createNotificationChannel();

            // Construct a back stack for cross-task navigation:
            var stackBuilder = Android.App.TaskStackBuilder.Create(m_Context);
            stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(TabsActivity)));
            stackBuilder.AddNextIntent(m_ResultIntent);

            // Create the PendingIntent with the back stack:            
            var resultPendingIntent = stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);

            // Build the notification:
            var builder = new NotificationCompat.Builder(m_Context, CHANNEL_ID)
                          .SetAutoCancel(true) // Dismiss the notification from the notification area when the user clicks on it
                          .SetContentIntent(resultPendingIntent) // Start up this activity when the user clicks the intent.
                          .SetContentTitle(title) // Set the title
                          .SetSmallIcon(Resource.Drawable.MainLogo)
                          .SetContentText(message); // the message to display.

            // Finally, publish the notification:
            var notificationManager = NotificationManagerCompat.From(m_Context);
            notificationManager.Notify(NOTIFICATION_ID, builder.Build());
        }

        public void createNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification 
                // channel on older versions of Android.
                return;
            }

            var name = "mARK-It Notification";
            var channel = new NotificationChannel(CHANNEL_ID, name, NotificationImportance.High)
            {
                Description = string.Empty
            };

            var notificationManager = (NotificationManager)m_Context.GetSystemService(Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }
    }
}
