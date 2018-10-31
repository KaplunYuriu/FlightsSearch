using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightsSearch.Entities;
using Graph.Entities;

namespace FlightsSearch.Search
{
    public class AirportNode : AsyncNode<Airport>
    {
        public AirportNode(Airport airport) : base(airport)
        {
            Name = airport?.Name;
        }

        private List<Edge<Airport>> _edges;
        public override async Task<List<Edge<Airport>>> GetEdges()
        {
            if (Data == null)
                return await Task.FromResult(new List<Edge<Airport>>());

            if (_edges != null)
                return _edges;

            var routes = await Data.GetRoutes();
            _edges = routes.Where(x => x.Destination != null).Select(r => new Edge<Airport>
            {
                ConnectedNode = new AirportNode(r.Destination),
                Cost = r.Distance
            }).ToList();

            return _edges;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var node = (AirportNode)obj;
            return Data.Equals(node.Data);
        }

        public override int GetHashCode()
        {
            return Data.GetHashCode();
        }
    }
}