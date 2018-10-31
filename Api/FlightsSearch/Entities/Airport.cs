using System.Collections.Generic;
using System.Threading.Tasks;
using FlightsSearch.Providers;

namespace FlightsSearch.Entities
{
    public class Airport : Address
    {
        public static IRoutesProvider RoutesProvider;

        public string Name { get; set; }
        public string Alias { get; set; }
        public int Altitude { get; set; }
        public override string DisplayName => $"{Name} [{Alias}]";

        public async Task<List<Route>> GetRoutes()
        {
            return await RoutesProvider.GetRoutes(this);
        }

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

        public override string ToString()
        {
            return Alias;
        }
    }
}