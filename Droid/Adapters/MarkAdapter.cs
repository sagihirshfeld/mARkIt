using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using mARkIt.Models;

namespace mARkIt.Droid.Adapters
{
    class MarkAdapter : BaseAdapter
    {
        private Context context;
        private List<Mark> m_Marks;

        public MarkAdapter(Context context, List<Mark> marks)
        {
            this.context = context;
            m_Marks = marks;
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            MarkAdapterViewHolder holder = null;

            if (view != null)
                holder = view.Tag as MarkAdapterViewHolder;

            if (holder == null)
            {
                holder = new MarkAdapterViewHolder();
                var inflater = context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
                //replace with your item and your holder items
                //comment back in
                view = inflater.Inflate(Resource.Layout.MyMarks, parent, false);
                holder.Message = view.FindViewById<TextView>(Resource.Id.LocationMessageTextView);
                holder.Date = view.FindViewById<TextView>(Resource.Id.LocationDateTextView);

                view.Tag = holder;
            }


            //fill in your items
            var mark = m_Marks[position];
            holder.Message.Text = mark.Message;
            holder.Date.Text = mark.CreatedAt.ToLocalTime().ToLongDateString();

            return view;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return m_Marks.Count;
            }
        }

    }

    class MarkAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        public TextView Message { get; set; }
        public TextView Date { get; set; }
    }
}