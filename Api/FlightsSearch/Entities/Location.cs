namespace FlightsSearch.Entities
{
    public class Location : Address
    {
        public int Id { get; set; }

        public override string DisplayName => City;
    }
}