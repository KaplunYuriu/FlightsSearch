using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightsSearch.Entities;
using FlightsSearch.Providers;
using FlightsSearch.Search;
using FlightsSearch.Search.Graph;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
            var engine = new SearchEngine(new AirportNode(departure), new AirportNode(destination));
            return await engine.Search();

            var routes = await GetRoutesForAirport(departure);
            
            return routes.Where(r => r.Destination.Equals(destination)).ToList();
        }
    }
}