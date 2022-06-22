using Microsoft.Azure.Mobile.Server;

namespace mARkIt.Backend.DataObjects
{
    public class Mark : EntityData
    {
        public string UserId { get; set; }
        public string Message { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Altitude { get; set; }
        public string Style { get; set; }
        public int CategoriesCode { get; set; }
        public float RatingsSum { get; set; }
        public int RatingsCount { get; set; }
        public float Rating { get; set; }

        public void UpdateRating()
        {
            Rating = RatingsSum / RatingsCount;
        }
    }
}