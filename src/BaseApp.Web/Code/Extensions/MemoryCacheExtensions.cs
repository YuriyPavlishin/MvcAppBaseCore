using System;
using Microsoft.Extensions.Caching.Memory;

namespace BaseApp.Web.Code.Extensions
{
    public static class MemoryCacheExtensions
    {
        public static T GetOrAdd<T>(this IMemoryCache memoryCache, string key, Func<T> getValueFunc, MemoryCacheEntryOptions options = null)
        {
            T result;
            if (memoryCache.TryGetValue(key, out result))
                return result;
            result = getValueFunc();
            memoryCache.Set(key, result, options ?? new MemoryCacheEntryOptions() {AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20)});

            return result;
        }
    }
}
