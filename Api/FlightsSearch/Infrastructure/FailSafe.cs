using System;
using System.Threading.Tasks;
using NLog;

namespace FlightsSearch.Infrastructure
{
    public static class FailSafe
    {
        public static async Task<T> TryTwice<T>(Func<Task<T>> action)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex, "Failed to execute action, trying one more time.");
                try
                {
                    return await action();
                }
                catch (Exception innerEx)
                {
                    LogManager.GetCurrentClassLogger().Error(innerEx, "Failed second time, throwing ex.");
                    throw;
                }
            }
        }
    }
}