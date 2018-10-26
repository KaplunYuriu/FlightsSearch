using System;
using FlightsSearch.Entities;

namespace FlightsSearch.Infrastructure
{
    //https://andrew.hedges.name/experiments/haversine/
    public static class DistanceCalculator
    {
        public static double GetDistance(Address firstLocation, Address secondLocation)
        {
            const int earthRadius = 6373;

            var lat1 = ToRadian(firstLocation.Latitude);
            var lon1 = ToRadian(firstLocation.Longitude);
            var lat2 = ToRadian(secondLocation.Latitude);
            var lon2 = ToRadian(secondLocation.Longitude);

            var dlon = lat2 - lat1;
            var dlat = lon2 - lon1;

            var a = Math.Pow(Math.Sin(dlat / 2), 2) + Math.Cos(firstLocation.Latitude) * Math.Cos(secondLocation.Latitude) * Math.Pow(Math.Sin(dlon / 2), 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = earthRadius * c;

            return d;
        }

        private static double ToRadian(double degrees)
        {
            return degrees * Math.PI/180;
        }
    }
}