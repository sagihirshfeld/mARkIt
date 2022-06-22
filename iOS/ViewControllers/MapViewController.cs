using MapKit;
using System;
using UIKit;
using mARkIt.Models;
using CoreLocation;
using System.Threading.Tasks;
using mARkIt.Utils;
using mARkIt.iOS.CustomControls;

namespace mARkIt.iOS
{
    public partial class MapViewController : UIViewController
    {
        private bool m_UserLocationInit = false;

        public MapViewController (IntPtr handle) : base (handle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            mapView.DidUpdateUserLocation += MapView_DidUpdateUserLocation;
            mapView.GetViewForAnnotation = GetViewForAnnotation;
            mapView.ShowsCompass = true;
        }

        public MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            MKAnnotationView pinView = null;
            if (annotation is MarkAnnotation)
            {
                pinView = mapView.DequeueReusableAnnotation("PinAnnotation");
                if (pinView == null)
                    pinView = new MKAnnotationView(annotation, "PinAnnotation");
                pinView.Image = GetIconByCategory((annotation as MarkAnnotation).Category);
                pinView.CanShowCallout = true;
            }

            return pinView;
        }

        private UIImage GetIconByCategory(eCategories i_Category)
        {
            UIImage icon = null;
            switch (i_Category)
            {
                case eCategories.General:
                    icon = UIImage.FromBundle("mapPins/General.png");
                    break;
                case eCategories.Food:
                    icon = UIImage.FromBundle("mapPins/Food.png");
                    break;
                case eCategories.Sport:
                    icon = UIImage.FromBundle("mapPins/Sport.png");
                    break;
                case eCategories.History:
                    icon = UIImage.FromBundle("mapPins/History.png");
                    break;
                case eCategories.Nature:
                    icon = UIImage.FromBundle("mapPins/Nature.png");
                    break;
                default:
                    icon = UIImage.FromBundle("mapPins/General.png");
                    break;
            }

            return icon;
        }

        public override async void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            await getPins();
        }

        private void MapView_DidUpdateUserLocation(object sender, MKUserLocationEventArgs e)
        {
            if(!m_UserLocationInit)
            {
                m_UserLocationInit = true;
                var coordinateSpan = new MKCoordinateSpan(0.01, 0.01);
                var coordinateRegion = new MKCoordinateRegion(mapView.UserLocation.Coordinate, coordinateSpan);
                mapView.SetRegion(coordinateRegion, false);
            }
        }


        private async Task getPins()
        { 
            var marks = await Mark.GetRelevantMarks();
            if (marks != null)
            {
                mapView.RemoveAnnotations(mapView.Annotations);
                foreach (Mark mark in marks)
                {
                    var pin = new MarkAnnotation()
                    {
                        Coordinate = new CLLocationCoordinate2D(mark.Latitude, mark.Longitude),
                        Category = (eCategories)mark.CategoriesCode
                    };
                    mapView.AddAnnotation(pin);
                }
            }

        }

    }
}