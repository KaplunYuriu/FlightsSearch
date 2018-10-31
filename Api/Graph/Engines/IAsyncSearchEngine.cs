using System.Threading.Tasks;
using Graph.Entities;

namespace Graph.Engines
{
    public interface IAsyncSearchEngine<T>
    {
        Task<Map<T>> GetMap(AsyncNode<T> start, AsyncNode<T> end);
    }
}