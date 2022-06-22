using System;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Widget;
using mARkIt.Droid.Helpers;
using mARkIt.Models;
using Newtonsoft.Json;

namespace mARkIt.Droid.Activities
{
    [Activity(Label = "MarkDetailsActivity")]
    public class MarkDetailsActivity : Activity, IOnMapReadyCallback
    {
        private MapFragment m_MapFragment;
        private TextView m_MessageTextView;
        private TextView m_DateTextView;
        private RatingBar m_MarkRatingBar;
        private Mark m_Mark;
        private GoogleMap m_GoogleMap;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            SetContentView(Resource.Layout.MarkPresentaion);
            string markAsJson = Intent.GetStringExtra("markAsJson");
            m_Mark  = JsonConvert.DeserializeObject<Mark>(markAsJson);
            Button button = FindViewById<Button>(Resource.Id.DeleteButton);
            button.Click += deleteButton_Click;
            findComponents();
            m_MessageTextView.Text = m_Mark.Message;
            m_DateTextView.Text = m_Mark.CreatedAt.ToLocalTime().ToLongDateString();
            m_MarkRatingBar.Rating = m_Mark.Rating;
            m_MapFragment.GetMapAsync(this);
        }

        private void findComponents()
        {
            m_MessageTextView = FindViewById<TextView>(Resource.Id.MessageTextView1);
            m_DateTextView = FindViewById<TextView>(Resource.Id.DateTextView1);
            m_MarkRatingBar = FindViewById<RatingBar>(Resource.Id.MarkRatingBar);
            m_MapFragment = FragmentManager.FindFragmentById<MapFragment>(Resource.Id.MapFragment);
            m_MarkRatingBar.Rating = m_Mark.Rating;
        }

        private async void deleteButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            button.Clickable = false;
            bool deleted = await Mark.Delete(m_Mark);
            if(deleted)
            {
                Alert.Show("Success", "Mark deleted successfully", this, Finish);
            }
            else
            {
                Alert.Show("Failure", "Mark couldnt be deleted", this, () => button.Clickable = true);
            }
        }      

        public void OnMapReady(GoogleMap googleMap)
        {
            m_GoogleMap = googleMap;
            mapToMarkLocation();
        }

        private void mapToMarkLocation()
        {
            MarkerOptions marker = new MarkerOptions();
            LatLng position = new LatLng(m_Mark.Latitude, m_Mark.Longitude);
            marker.SetPosition(position);
            marker.SetTitle(m_Mark.Message);
            m_GoogleMap.AddMarker(marker);
            var cameraPosition = new Android.Gms.Maps.Model.CameraPosition.Builder().Target(position).Zoom(16).Bearing(0).Build();
            m_GoogleMap.MoveCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition));
        }
    }
}