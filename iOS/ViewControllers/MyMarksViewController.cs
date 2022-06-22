using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using mARkIt.Models;
using Syncfusion.SfRating.iOS;

namespace mARkIt.iOS
{
    public partial class MyMarksViewController : UITableViewController
    {
        private List<Mark> m_Marks;

        public MyMarksViewController (IntPtr handle) : base (handle)
        {
            m_Marks = new List<Mark>();
        }

        public override async void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            var marks = await Mark.GetMyMarks();
            if(marks != null)
            {
                m_Marks = marks;
                TableView.ReloadData();
            }
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return m_Marks.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell("markCell") as MarkTableViewCell;
            var mark = m_Marks[indexPath.Row];
            cell.MessageLabel.Text = mark.Message;
            cell.DateLabel.Text  = mark.CreatedAt.ToLocalTime().ToLongDateString();
            cell.RatingBar.ItemSize = 10;
            cell.RatingBar.UserInteractionEnabled = false;
            cell.RatingBar.Precision = SFRatingPrecision.Exact;
            cell.RatingBar.Value = mark.Rating;

            return cell;
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if(segue.Identifier == "markSegue")
            {
                var selectedRow = TableView.IndexPathForSelectedRow;
                var destenationViewController = segue.DestinationViewController as MarkViewController;
                destenationViewController.ViewMark = m_Marks[selectedRow.Row];
            }
            base.PrepareForSegue(segue, sender);
        }
    }
}