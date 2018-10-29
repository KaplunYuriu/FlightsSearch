using System.Collections.Generic;
using System.Threading.Tasks;
using FlightsSearch.Entities;
using FlightsSearch.Providers;

namespace FlightsSearch.Services
{
    public interface IRoutesService
    {
        Task<List<Route>> GetRoutesForAirport(Airport airport);
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
    }
}