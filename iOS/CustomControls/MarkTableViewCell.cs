using Syncfusion.SfRating.iOS;
using System;
using UIKit;

namespace mARkIt.iOS
{
    public partial class MarkTableViewCell : UITableViewCell
    {
        public MarkTableViewCell (IntPtr handle) : base (handle)
        {
        }

        public UILabel CoordinatesLabel
        {
            get
            {
                return coordinatesLabel;
            }
        }

        public UILabel DateLabel
        {
            get
            {
                return dateLabel;
            }
          
        }

        public UILabel MessageLabel
        {
            get
            {
                return messageLabel;
            }
        }

        public SfRating RatingBar
        {
            get
            {
                return ratingBar;
            }
        }
    }
}
