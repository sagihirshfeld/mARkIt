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
    [Register ("MarkViewController")]
    partial class MarkViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem backBarButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel dateLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem deleteMarkButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MapKit.MKMapView mapView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView messageTextView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel numOfRatersLabels { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Syncfusion.SfRating.iOS.SfRating ratingBar { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (backBarButton != null) {
                backBarButton.Dispose ();
                backBarButton = null;
            }

            if (dateLabel != null) {
                dateLabel.Dispose ();
                dateLabel = null;
            }

            if (deleteMarkButton != null) {
                deleteMarkButton.Dispose ();
                deleteMarkButton = null;
            }

            if (mapView != null) {
                mapView.Dispose ();
                mapView = null;
            }

            if (messageTextView != null) {
                messageTextView.Dispose ();
                messageTextView = null;
            }

            if (numOfRatersLabels != null) {
                numOfRatersLabels.Dispose ();
                numOfRatersLabels = null;
            }

            if (ratingBar != null) {
                ratingBar.Dispose ();
                ratingBar = null;
            }
        }
    }
}