using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightsSearch.Entities;

namespace FlightsSearch.Search.Graph
{
    public abstract class Node<T>
    {
        protected T Data;

        public string Name { get; protected set; }

        public double? MinCostToStart { get; set; }

        public bool Visited { get; set; }

        public AirportNode NearestToStart { get; set; }

        protected Node(T data)
        {
            Data = data;
        }
    }

    public class AirportNode : Node<Airport>
    {
        public AirportNode(Airport airport) : base(airport)
        {
            Name = airport?.Name;
        }

        private List<Edge<Airport>> _edges;

        public async Task<List<Edge<Airport>>> GetEdges()
        {
            if (Data == null)
                return await Task.FromResult(new List<Edge<Airport>>());

            if (_edges != null)
                return _edges;

            var routes = await Data.GetRoutes();
            _edges = routes.Where(x => x.Destination != null).Select(r => new Edge<Airport>
            {
                ConnectedNode = new AirportNode(r.Destination)
            }).ToList();

            return _edges;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var node = (AirportNode) obj;
            return Data.Equals(node.Data);
        }

        public override int GetHashCode()
        {
            return Data.GetHashCode();
        }
    }

    public class Edge<T>
    {
        private static Random _rand = new Random();

        public double Length { get; set; }
        public double Cost { get; private set; }
        public AirportNode ConnectedNode { get; set; }

        public Edge()
        {
            Cost = _rand.Next(500);
        }
    }
}
