namespace FlightsSearch.Entities
{
    public class Airport : Address
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public int Altitude { get; set; }
    }
}