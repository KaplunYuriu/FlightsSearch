using FlightsSearch.Search.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightsSearch.Entities;

namespace FlightsSearch.Search
{
    public class SearchEngine
    {
        private AirportNode Start;
        private AirportNode End;

        private int NodeVisits;
        public double ShortestPathLenght;
        public double ShortestPathCost;

        public SearchEngine(AirportNode start, AirportNode end)
        {
            Start = start;
            End = end;
        }

        private async Task SearchInGraph()
        {
            NodeVisits = 0;

            Start.MinCostToStart = 0;

            var prioQueue = new List<AirportNode>
            {
                Start
            };

            do
            {
                NodeVisits++;
                prioQueue = prioQueue.OrderBy(x => x.MinCostToStart.Value).ToList();

                var node = prioQueue.First();
                prioQueue.Remove(node);
                foreach (var edge in await node.GetEdges())
                {
                    var childNode = edge.ConnectedNode;
                    if (childNode == null || childNode.Visited)
                        continue;

                    if (childNode.MinCostToStart == null ||
                        node.MinCostToStart + edge.Cost < childNode.MinCostToStart)
                    {
                        childNode.MinCostToStart = node.MinCostToStart + edge.Cost;
                        childNode.NearestToStart = node;
                        if (!prioQueue.Contains(childNode))
                            prioQueue.Add(childNode);
                    }
                }

                node.Visited = true;

                if (node.Equals(End))
                {
                    End = node;
                    return;
                }
            } while (prioQueue.Any());
        }

        private async Task BuildShortestPath(List<AirportNode> list, AirportNode node)
        {
            if (node.NearestToStart == null)
                return;
            list.Add(node.NearestToStart);
            var edges = await node.GetEdges();

            ShortestPathLenght += edges.Single(x => x.ConnectedNode.Equals(node.NearestToStart)).Length;
            ShortestPathCost += edges.Single(x => x.ConnectedNode.Equals(node.NearestToStart)).Cost;

            await BuildShortestPath(list, node.NearestToStart);
        }

        public async Task<List<Route>> Search()
        {
            await SearchInGraph();

            var shortestPath = new List<AirportNode>();
            shortestPath.Add(End);
            Task.WaitAny(BuildShortestPath(shortestPath, End));

            shortestPath.Reverse();
            
            return new List<Route>();
        }
    }
}
