namespace FlightsSearch.Entities
{
    public class Route
    {
        public Airport Departure { get; set; }

        public Airport Destination { get; set; }

        public Airline Airline { get; set; }

        public double Distance => Departure.DistanceTo(Destination);
    }
}