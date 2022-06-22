using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using mARkIt.Droid.Activities;
using mARkIt.Droid.Adapters;
using mARkIt.Models;
using Newtonsoft.Json;

namespace mARkIt.Droid.Fragments
{
    public class MyMarksFragment : Android.Support.V4.App.ListFragment
    {
        List<Mark> m_Marks;

        // To prevent double clicking an item
        bool m_ClickResponseEnabled = true;

        private async void getMyMarks()
        {
            m_Marks = await Mark.GetMyMarks();
            if(m_Marks!=null)
            {
                ListAdapter = new MarkAdapter(Context, m_Marks);
            }
        }

        public override void OnHiddenChanged(bool hidden)
        {
            base.OnHiddenChanged(hidden);
            if (!hidden)
            {
                getMyMarks();
            }

            m_ClickResponseEnabled = true;
        }

        public override void OnResume()
        {
            base.OnResume();
            if(!IsHidden)
            {
                getMyMarks();
            }

            m_ClickResponseEnabled = true;
        }

        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            base.OnListItemClick(l, v, position, id);

            if (m_ClickResponseEnabled)
            {
                var selectedMark = m_Marks[position];
                Intent intent = new Intent(Activity, typeof(MarkDetailsActivity));
                string MarkAsJson = JsonConvert.SerializeObject(selectedMark);
                intent.PutExtra("markAsJson", MarkAsJson);
                StartActivity(intent);
                m_ClickResponseEnabled = false;
            }
        }
    }
}