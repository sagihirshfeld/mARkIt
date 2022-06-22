using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows;

namespace mARkIt.Backend.Utils
{
    public static class DistanceCalculator
    {
        private const double k_EarthRadius = 6371e3;

        // Derived from the article https://www.movable-type.co.uk/scripts/latlong.html
        public static double DistanceInKm(Vector pos1, Vector pos2)
        {
            double lat1Rad = toRadians(pos1.X);
            double lat2Rad = toRadians(pos2.X);
            double latDeltaRad = toRadians(pos1.X - pos2.X);
            double lonDeltaRad = toRadians(pos1.Y - pos2.Y);

            double calculatedValue1 = Math.Sin(latDeltaRad / 2) * Math.Sin(latDeltaRad / 2) +
                    Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                    Math.Sin(lonDeltaRad / 2) * Math.Sin(lonDeltaRad / 2);
            double calculatedValue2 = 2 * Math.Atan2(Math.Sqrt(calculatedValue1), Math.Sqrt(1 - calculatedValue1));

            //Based on earth radius, in km
            double distance = (k_EarthRadius * calculatedValue2) / 1000;

            return distance;
        }

        private static double toRadians(double degree)
        {
            return degree * Math.PI / 180;
        }
    }
}