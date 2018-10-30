using System.Drawing;

namespace FlightsSearch.Entities
{
    public class Airport : Address
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public int Altitude { get; set; }
        public override string DisplayName => $"{Name} [{Alias}]";

        public override int GetHashCode()
        {
            return Alias.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var aiprort = (Airport)obj;
            return aiprort.Alias.Equals(Alias);
        }
    }
}