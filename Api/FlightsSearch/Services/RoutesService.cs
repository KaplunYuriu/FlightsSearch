using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightsSearch.Entities;
using FlightsSearch.Providers;

namespace FlightsSearch.Services
{
    public interface IRoutesService
    {
        Task<List<Route>> GetRoutesForAirport(Airport airport);

        Task<List<Route>> GetRoutesBetween(Airport departure, Airport destination);
    }

    public class RoutesService : IRoutesService
    {
        private readonly IRoutesProvider _routesProvider;

        public RoutesService(IRoutesProvider routesProvider)
        {
            _routesProvider = routesProvider;
        }

        public async Task<List<Route>> GetRoutesForAirport(Airport airport)
        {
            return await _routesProvider.GetRoutes(airport);
        }

        public async Task<List<Route>> GetRoutesBetween(Airport departure, Airport destination)
        {
            var routes = await GetRoutesForAirport(departure);


            return routes.Where(r => r.Destination.Equals(destination)).ToList();
        }
    }
}