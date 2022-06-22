// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace mARkIt.iOS
{
    [Register ("SettingsViewController")]
    partial class SettingsViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Syncfusion.iOS.Buttons.SfCheckBox foodCheckBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Syncfusion.iOS.Buttons.SfCheckBox generalCheckBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Syncfusion.iOS.Buttons.SfCheckBox historyCheckBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton logoutButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Syncfusion.iOS.Buttons.SfCheckBox natureCheckBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem saveButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Syncfusion.iOS.Buttons.SfCheckBox sportCheckBox { get; set; }

        [Action ("UIBarButtonItem36992_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void UIBarButtonItem36992_Activated (UIKit.UIBarButtonItem sender);

        void ReleaseDesignerOutlets ()
        {
            if (foodCheckBox != null) {
                foodCheckBox.Dispose ();
                foodCheckBox = null;
            }

            if (generalCheckBox != null) {
                generalCheckBox.Dispose ();
                generalCheckBox = null;
            }

            if (historyCheckBox != null) {
                historyCheckBox.Dispose ();
                historyCheckBox = null;
            }

            if (logoutButton != null) {
                logoutButton.Dispose ();
                logoutButton = null;
            }

            if (natureCheckBox != null) {
                natureCheckBox.Dispose ();
                natureCheckBox = null;
            }

            if (saveButton != null) {
                saveButton.Dispose ();
                saveButton = null;
            }

            if (sportCheckBox != null) {
                sportCheckBox.Dispose ();
                sportCheckBox = null;
            }
        }
    }
}