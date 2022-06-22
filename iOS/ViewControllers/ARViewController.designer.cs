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
    [Register ("ARViewController")]
    partial class ARViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton addMarkButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (addMarkButton != null) {
                addMarkButton.Dispose ();
                addMarkButton = null;
            }
        }
    }
}