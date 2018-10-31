using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Graph.Entities;
using NLog;

namespace Graph.Engines
{
    class AsyncDijkstraSearchEngine<T> : IAsyncSearchEngine<T>
    {
        private int _nodeVisits;
        public double ShortestPathLenght;
        public double ShortestPathCost;

        private Map<T> _resultMap;

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        
        public async Task<Map<T>> GetMap(AsyncNode<T> start, AsyncNode<T> end)
        {
            _resultMap = new Map<T>
            {
                Start = start,
                End = end
            };

            await ProcessNodes();

            var shortestPath = new List<AsyncNode<T>> {_resultMap.End};

            await BuildShortestPath(shortestPath, _resultMap.End);

            shortestPath.Reverse();

            _resultMap.Path = shortestPath;

            return _resultMap;
        }

        private async Task ProcessNodes()
        {
            var log = new Action<string>((message) =>
                _logger.Debug($"Dijsktra Log: {message}"));


            _nodeVisits = 0;

            _resultMap.Start.MinCostToStart = 0;

            var prioQueue = new List<AsyncNode<T>>
            {
                _resultMap.Start
            };

            log($"Starting search from `{_resultMap.Start}` to {_resultMap.End}.");

            var visitedNotes = new HashSet<AsyncNode<T>>();

            do
            {
                _nodeVisits++;
                prioQueue = prioQueue.OrderBy(x => x.MinCostToStart.Value).ToList();

                var node = prioQueue.First();
                log($"Current node is `{node}`. Queue size: `{prioQueue.Count}`");

                prioQueue.Remove(node);
                foreach (var edge in await node.GetEdges())
                {
                    log($"Current edge is `{edge.ConnectedNode.Data}` for node `{node}`.");
                    var childNode = edge.ConnectedNode;
                    if (childNode == null || visitedNotes.Contains(childNode))
                    {
                        log($"Edge `{edge.ConnectedNode.Data}` has been visited already.");
                        continue;
                    }

                    if (childNode.MinCostToStart == null ||
                        node.MinCostToStart + edge.Cost < childNode.MinCostToStart)
                    {
                        childNode.MinCostToStart = node.MinCostToStart + edge.Cost;
                        childNode.NearestToStart = node;
                        log($"Adding edge `{edge.ConnectedNode.Data}` from node `{node}`.");
                        if (!prioQueue.Contains(childNode))
                            prioQueue.Add(childNode);
                    }
                }

                visitedNotes.Add(node);

                if (node.Equals(_resultMap.End))
                {
                    log($"Found solution. Nodes visited: `{_nodeVisits}`.");
                    _resultMap.End = node;
                    return;
                }
            } while (prioQueue.Any());
        }

        private async Task BuildShortestPath(List<AsyncNode<T>> list, AsyncNode<T> node)
        {
            if (node.NearestToStart == null)
                return;
            list.Add(node.NearestToStart);
            var edges = await node.GetEdges();

            ShortestPathLenght += edges.Single(x => x.ConnectedNode.Equals(node.NearestToStart)).Length;
            ShortestPathCost += edges.Single(x => x.ConnectedNode.Equals(node.NearestToStart)).Cost;

            await BuildShortestPath(list, node.NearestToStart);
        }
    }
}
