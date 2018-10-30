using System;

namespace FlightsSearch.Entities
{
    public abstract class Address
    {
        public string City { get; set; }
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public abstract string DisplayName { get; }

        //https://andrew.hedges.name/experiments/haversine/
        public double DistanceTo(Address location)
        {
            const int earthRadius = 6373;

            var lat1 = ToRadian(Latitude);
            var lon1 = ToRadian(Longitude);
            var lat2 = ToRadian(location.Latitude);
            var lon2 = ToRadian(location.Longitude);

            var dlon = lat2 - lat1;
            var dlat = lon2 - lon1;

            var a = Math.Pow(Math.Sin(dlat / 2), 2) + Math.Cos(Latitude) * Math.Cos(Latitude) * Math.Pow(Math.Sin(dlon / 2), 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = earthRadius * c;

            return d;
        }

        private double ToRadian(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }
}