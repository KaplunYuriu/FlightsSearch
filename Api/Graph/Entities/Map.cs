using System.Collections.Generic;
using System.Linq;

namespace Graph.Entities
{
    public class Map<T>
    {
        public AsyncNode<T> Start { get; set; }

        public AsyncNode<T> End { get; set; }

        public List<AsyncNode<T>> Path { get; set; }

        public bool HasPath => Path != null && Path.Any();

        public List<T> Values => Path.Select(x => x.Data).ToList();
    }
}