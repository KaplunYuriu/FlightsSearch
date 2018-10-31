using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Graph.Entities;

namespace Graph.Engines
{
    class AsyncDijkstraSearchEngine<T> : IAsyncSearchEngine<T>
    {
        private int NodeVisits;
        public double ShortestPathLenght;
        public double ShortestPathCost;

        private Map<T> _resultMap;
        
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
            NodeVisits = 0;

            _resultMap.Start.MinCostToStart = 0;

            var prioQueue = new List<AsyncNode<T>>
            {
                _resultMap.Start
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

                if (node.Equals(_resultMap.End))
                {
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
