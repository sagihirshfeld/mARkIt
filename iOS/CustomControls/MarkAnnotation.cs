using System;
using MapKit;
using mARkIt.Utils;

namespace mARkIt.iOS.CustomControls
{
    public class MarkAnnotation : MKPointAnnotation
    {       
        public eCategories Category { get; set; }
    }
}
