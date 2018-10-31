using System.Collections.Generic;
using System.Threading.Tasks;
using FlightsSearch.Entities;
using FlightsSearch.Providers;
using FlightsSearch.Search;
using Graph.Engines;
using Graph.Entities;

namespace FlightsSearch.Services
{
    public interface IRoutesService
    {
        Task<List<Route>> GetRoutesBetween(Airport departure, Airport destination);
    }

    public class RoutesService : IRoutesService
    {
        private readonly IRoutesProvider _routesProvider;

        public RoutesService(IRoutesProvider routesProvider)
        {
            _routesProvider = routesProvider;
        }

        public async Task<List<Route>> GetRoutesBetween(Airport departure, Airport destination)
        {
            var searchEngine = SearchEngineFactory<Airport>.GetSearchEngine(SearchAlgorithm.Dijkstra);
            var map = await searchEngine.GetMap(new AirportNode(departure), new AirportNode(destination));

            var requiredRoutes = await ProcessRoutesInPath(map);

            return requiredRoutes;
        }

        private async Task<List<Route>> ProcessRoutesInPath(Map<Airport> map)
        {
            if (!map.HasPath)
                return await Task.FromResult(new List<Route>());

            var path = new List<Route>();

            for (int i = 0; i < map.Values.Count - 1; i++)
            {
                var departureAirport = map.Values[i];
                var destinationAirport = map.Values[i + 1];

                var routes = await departureAirport.GetRoutes();
                path.Add(routes.Find(x => x.Departure.Equals(departureAirport) && (x.Destination != null && x.Destination.Equals(destinationAirport))));
            }

            return path;
        }
    }
}