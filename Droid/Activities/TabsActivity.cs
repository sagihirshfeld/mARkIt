using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using mARkIt.Droid.Fragments;

namespace mARkIt.Droid.Activities
{
    [Activity(Label = "TabsActivity", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.KeyboardHidden | Android.Content.PM.ConfigChanges.ScreenSize)]

    public class TabsActivity : FragmentActivity
    {
        private TabLayout m_TabLayout;
        private ExploreFragment m_ExploreFragment;
        private MapFragment m_MapFragment;
        private SettingsFragment m_SettingsFragment;
        private MyMarksFragment m_MyMarksFragment;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            SetContentView(Resource.Layout.Tabs);

            m_TabLayout = FindViewById<TabLayout>(Resource.Id.mainTabLayout);
            m_TabLayout.TabSelected += TabLayout_TabSelected;
            createTabLayoutIconsAndText();

            // create fragments
            m_ExploreFragment = new ExploreFragment();
            m_MapFragment = new MapFragment();
            m_SettingsFragment = new SettingsFragment();
            m_MyMarksFragment = new MyMarksFragment();
            navigateToFragment(m_ExploreFragment);
        }

        private void TabLayout_TabSelected(object sender, TabLayout.TabSelectedEventArgs e)
        {
            switch(e.Tab.Position)
            {
                case 0:
                    navigateToFragment(m_ExploreFragment);
                    break;
                case 1:
                    navigateToFragment(m_MapFragment);
                    break;
                case 2:
                    navigateToFragment(m_MyMarksFragment);
                    break;
                case 3:
                    navigateToFragment(m_SettingsFragment);
                    break;
            }
        }

        Android.Support.V4.App.Fragment m_CurrentFragment;

        private void navigateToFragment(Android.Support.V4.App.Fragment fragment)
        {            
            // If the fragment hasn't been added yet - add it
            if (SupportFragmentManager.FindFragmentById(fragment.Id) == null)
            {
                SupportFragmentManager.BeginTransaction().Add(Resource.Id.contentFrame, fragment).Commit();
            }

            // Show the new fragment
            SupportFragmentManager.BeginTransaction().Show(fragment).Commit();

            // Hide the previous fragment
            if (m_CurrentFragment != null && m_CurrentFragment != fragment)
            {
                SupportFragmentManager.BeginTransaction().Hide(m_CurrentFragment).Commit();
            }

            m_CurrentFragment = fragment;
        }

        public override void OnBackPressed()
        {
            //base.OnBackPressed();
        }

        private void createTabLayoutIconsAndText()
        {
            m_TabLayout.GetTabAt(0).SetIcon(Resource.Drawable.Explore);
            m_TabLayout.GetTabAt(1).SetIcon(Resource.Drawable.Map);
            m_TabLayout.GetTabAt(2).SetIcon(Resource.Drawable.MyMarks);
            m_TabLayout.GetTabAt(3).SetIcon(Resource.Drawable.Settings);
        }
    }
}
