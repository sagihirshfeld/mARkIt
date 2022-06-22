using System;
using Android.App;
using Android.Content;

namespace mARkIt.Droid.Helpers
{
    class Alert
    {
        public static void Show(string i_Title, string i_Message, Context i_Context, Action i_Action = null)
        {
            AlertDialog.Builder dialog = new AlertDialog.Builder(i_Context);
            dialog.SetTitle(i_Title);
            dialog.SetMessage(i_Message);
            dialog.SetPositiveButton("OK", (sender, eventArgs) => i_Action?.Invoke());
            dialog.SetCancelable(false);
            dialog.Show();
        }
    }
}