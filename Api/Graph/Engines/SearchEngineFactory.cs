using System;

namespace Graph.Engines
{
    public enum SearchAlgorithm
    {
        Dijkstra = 1,
    }

    public static class SearchEngineFactory<T>
    {
        public static IAsyncSearchEngine<T> GetSearchEngine(SearchAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case SearchAlgorithm.Dijkstra:
                    return new AsyncDijkstraSearchEngine<T>();

                default: 
                    throw new ArgumentException();
            }
        }
    }
}