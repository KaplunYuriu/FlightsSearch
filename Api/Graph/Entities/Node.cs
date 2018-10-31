using System.Collections.Generic;
using System.Threading.Tasks;

namespace Graph.Entities
{
    public abstract class AsyncNode<T>
    {
        public T Data { get; }

        public string Name { get; protected set; }

        public double? MinCostToStart { get; set; }

        public bool Visited { get; set; }

        public AsyncNode<T> NearestToStart { get; set; }

        public abstract Task<List<Edge<T>>> GetEdges();

        protected AsyncNode(T data)
        {
            Data = data;
        }
    }
}
