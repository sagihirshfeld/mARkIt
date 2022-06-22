using Foundation;
using mARkIt.Notifications;
using UIKit;

namespace mARkIt.iOS.Notifications
{
    public class IOSLocalNotification : ILocalNotification
    {
        public void Show(string i_Title, string i_Message)
        {
            // create the notification
            var notification = new UILocalNotification();

            // set the fire date (the date time in which it will fire)
            notification.FireDate = NSDate.FromTimeIntervalSinceNow(0);

            // configure the alert
            notification.AlertAction = i_Title;
            notification.AlertBody = i_Message;

            // modify the badge
            notification.ApplicationIconBadgeNumber = 1;

            // set the sound to be the default sound
            notification.SoundName = UILocalNotification.DefaultSoundName;

            // schedule it
            UIApplication.SharedApplication.ScheduleLocalNotification(notification);
        }
    }
}