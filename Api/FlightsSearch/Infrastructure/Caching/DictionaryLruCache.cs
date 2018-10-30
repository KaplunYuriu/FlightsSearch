using System;
using System.Collections.Generic;

namespace FlightsSearch.Infrastructure.Caching
{
    public class DictionaryCacheElement<TValue>
    {
        public TValue Value;

        private DateTime _lastUpdated;
        private readonly TimeSpan _elementLifetime;

        public DictionaryCacheElement(TValue value, TimeSpan elementLifetime)
        {
            _elementLifetime = elementLifetime;
            UpdateValue(value);
        }

        public void UpdateValue(TValue value)
        {
            Value = value;
            _lastUpdated = DateTime.UtcNow;
        }

        public bool IsEpired => _lastUpdated + _elementLifetime < DateTime.UtcNow;
    }

    public class DictionaryLruCache<TKey, TValue> where TValue : class
    {
        private readonly Dictionary<TKey, DictionaryCacheElement<TValue>> _cachedResource = new Dictionary<TKey, DictionaryCacheElement<TValue>>();
        private readonly TimeSpan _elementLifetime;

        private readonly LinkedList<TKey> _lru = new LinkedList<TKey>();

        private readonly int _maxCacheSize;

        private readonly object _lock = new object();

        protected DictionaryLruCache(TimeSpan elementLifetime, int maxElements = 50)
        {
            _elementLifetime = elementLifetime;
            _maxCacheSize = maxElements;
        }

        protected TValue this[TKey key]
        {
            get
            {
                lock (_lock)
                {
                    if (!_cachedResource.TryGetValue(key, out var cacheElement))
                        return null;

                    if (cacheElement.IsEpired)
                    {
                        _cachedResource.Remove(_lru.Last.Value);
                        _lru.RemoveLast();

                        return null;
                    }

                    _lru.Remove(key);
                    _lru.AddFirst(key);

                    return cacheElement.Value;
                }
            }
            set
            {
                lock (_lock)
                {
                    if (_cachedResource.TryGetValue(key, out _))
                        _cachedResource[key].UpdateValue(value);
                    else
                    {
                        _cachedResource.Add(key, new DictionaryCacheElement<TValue>(value, _elementLifetime));
                        _lru.AddFirst(key);

                        if (_lru.Count > _maxCacheSize)
                        {
                            _cachedResource.Remove(_lru.Last.Value);
                            _lru.RemoveLast();
                        }
                    }
                }
            }
        }
    }
}