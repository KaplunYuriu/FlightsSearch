namespace FlightsSearch.Entities
{
    public class Route
    {
        public Airport Source { get; set; }

        public Airport Destination { get; set; }

        public Airline Airline { get; set; }
    }
}