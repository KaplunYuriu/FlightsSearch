using System;
using System.Timers;
using NLog;

namespace FlightsSearch.Infrastructure.Caching
{
    public abstract class AsyncCache<T>
        where T : class
    {
        /// <summary>
        /// To avoid all the caches refreshing at the exact same time after a restart, we introduce up to 10 seconds of randomness to the refresh timer
        /// </summary>
        private const int JITTER_RANGE_MS = 10000;

        private T _cachedResource;
        private bool _refreshing;
        private readonly object _cachelock = new object();

        private readonly Timer _refreshTimer;

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();


        internal AsyncCache(Timer refreshTimer)
        {
            _refreshing = false;

            _refreshTimer = refreshTimer;
            _refreshTimer.Elapsed += RefreshTimerElapsed;
        }

        protected AsyncCache(double cacheMinutes)
            : this(new Timer(cacheMinutes * 60 * 1000 + new Random().Next(JITTER_RANGE_MS)))
        {
        }

        private void RefreshTimerElapsed(object sender, ElapsedEventArgs e)
        {
            lock (_cachelock)
            {
                if (_refreshing)
                    return;

                _refreshTimer.Stop();
                Refresh();
            }
        }

        /// <summary>
        /// The user needs to implement this
        /// </summary>
        /// <returns></returns>
        public abstract T LoadData();


        private void Refresh()
        {
            try
            {
                _refreshing = true;
                _cachedResource = LoadData();
            }
            catch (Exception ex)
            {
                //If there is no dirty cache to fall back on, we let the exception bubble up. 
                if (_cachedResource == null)
                    throw;
                else
                {
                    _logger.Log(LogLevel.Error, ex);
                }
            }
            finally
            {
                _refreshTimer.Start();
                _refreshing = false;
            }
        }

        protected T CachedResource
        {
            get
            {
                if (_cachedResource == null)
                {
                    lock (_cachelock)
                    {
                        if (_cachedResource == null)
                        {
                            Refresh();
                        }
                    }
                }

                return _cachedResource;
            }
        }
    }
}