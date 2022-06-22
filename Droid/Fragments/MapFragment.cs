using Android.Gms.Maps;
using Android.OS;
using Android.Views;
using Android.Gms.Maps.Model;
using mARkIt.Models;
using mARkIt.Utils;

namespace mARkIt.Droid.Fragments
{
    public class MapFragment : Android.Support.V4.App.Fragment, IOnMapReadyCallback
    {
        private MapView m_MapView;
        private GoogleMap m_GoogleMap;
        private View m_View;
        private bool m_InitLocation = false;

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            m_MapView = m_View.FindViewById<MapView>(Resource.Id.mapView);
            if(m_MapView != null)
            {
                m_MapView.OnCreate(null);
                m_MapView.OnResume();
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            m_View = inflater.Inflate(Resource.Layout.Map, container, false);
            return m_View;
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            MapsInitializer.Initialize(Context);
            m_GoogleMap = googleMap;
            googleMap.MapType = GoogleMap.MapTypeNormal;
            googleMap.MyLocationEnabled = true;
            googleMap.Clear();
            addMarksFromServer();
            googleMap.MyLocationChange += GoogleMap_MyLocationChange;
        }

        private void GoogleMap_MyLocationChange(object sender, GoogleMap.MyLocationChangeEventArgs e)
        {
            if (!m_InitLocation)
            {
                mapToMyLocation();
                m_InitLocation = true;
            }
        }

        private async void addMarksFromServer()
        {
            var marks = await Mark.GetRelevantMarks();
            if (marks != null) 
            {
                foreach (Mark mark in marks) 
                {
                    MarkerOptions marker = new MarkerOptions();
                    marker.SetPosition(new LatLng(mark.Latitude, mark.Longitude));
                    marker.SetIcon(GetIconByCategory(mark.CategoriesCode));
                    m_GoogleMap.AddMarker(marker);
                }
            }
        }

        private BitmapDescriptor GetIconByCategory(int i_CategoriesCode)
        {
            BitmapDescriptor icon = null;
            if ((i_CategoriesCode & (int)eCategories.General) != 0) 
            {
                icon = BitmapDescriptorFactory.FromResource(Resource.Drawable.General);
            }
            else if((i_CategoriesCode & (int)eCategories.Food) != 0)
            {
                icon = BitmapDescriptorFactory.FromResource(Resource.Drawable.Food);
            }
            else if ((i_CategoriesCode & (int)eCategories.Sport) != 0)
            {
                icon = BitmapDescriptorFactory.FromResource(Resource.Drawable.Sport);
            }
            else if ((i_CategoriesCode & (int)eCategories.History) != 0)
            {
                icon = BitmapDescriptorFactory.FromResource(Resource.Drawable.History);
            }
            else if ((i_CategoriesCode & (int)eCategories.Nature) != 0)
            {
                icon = BitmapDescriptorFactory.FromResource(Resource.Drawable.Nature);
            }

            return icon;
        }

        private async void mapToMyLocation()
        {
            var geoInfo = await Plugin.Geolocator.CrossGeolocator.Current.GetLastKnownLocationAsync();
            if (geoInfo != null) 
            {
                LatLng position = new LatLng(geoInfo.Latitude, geoInfo.Longitude);
                var cameraPosition = new CameraPosition.Builder().Target(position).Zoom(16).Bearing(0).Build();
                m_GoogleMap.MoveCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition));
            }
        }


        public override void OnResume()
        {
            base.OnResume();
            m_InitLocation = false;
            m_MapView.GetMapAsync(this);
        }

        public override void OnHiddenChanged(bool hidden)
        {
            base.OnHiddenChanged(hidden);
            if (!hidden) 
            {
                m_InitLocation = false;
                m_MapView.GetMapAsync(this);
            }
        }
    }
}
