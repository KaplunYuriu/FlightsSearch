using System;
using System.Collections.Generic;

namespace FlightsSearch.Infrastructure.Caching
{
    public class DictionaryCacheElement<TValue>
    {
        public TValue Value;

        private DateTime _lastUpdated;

        public DictionaryCacheElement(TValue value)
        {
            UpdateValue(value);
        }

        public void UpdateValue(TValue value)
        {
            Value = value;
            _lastUpdated = DateTime.UtcNow;
        }

        public bool IsExpired(TimeSpan elementLifetime)
        {
            return _lastUpdated + elementLifetime < DateTime.UtcNow;
        }
    }

    public class DictionaryLruCache<TKey, TValue> where TValue : class
    {
        protected readonly Dictionary<TKey, DictionaryCacheElement<TValue>> CachedResource = new Dictionary<TKey, DictionaryCacheElement<TValue>>();
        protected readonly TimeSpan ElementLifetime;

        private readonly LinkedList<TKey> _lru = new LinkedList<TKey>();

        private readonly int _maxCacheSize;

        private readonly object _lock = new object();

        protected DictionaryLruCache(TimeSpan elementLifetime, int maxElements = 50)
        {
            ElementLifetime = elementLifetime;
            _maxCacheSize = maxElements;
        }

        protected TValue this[TKey key]
        {
            get
            {
                lock (_lock)
                {
                    if (!CachedResource.TryGetValue(key, out var cacheElement))
                        return null;

                    _lru.Remove(key);
                    _lru.AddFirst(key);

                    return cacheElement.Value;
                }
            }
            set
            {
                lock (_lock)
                {
                    if (CachedResource.TryGetValue(key, out var cacheElement))
                        CachedResource[key].UpdateValue(cacheElement.Value);
                    else
                    {
                        CachedResource.Add(key, new DictionaryCacheElement<TValue>(value));
                        _lru.AddFirst(key);

                        if (_lru.Count > _maxCacheSize)
                        {
                            CachedResource.Remove(_lru.Last.Value);
                            _lru.RemoveLast();
                        }
                    }
                }
            }
        }
    }
}