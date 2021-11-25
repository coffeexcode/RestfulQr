using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace RestfulQr.Api.Core.Caching
{
    public interface ICacheProvider<T>
    {
        public Task<T?> GetAsync(string key);

        public Task SetAsync(string key, T toCache);

        public Task RemoveAsync(string key);
    }

    public class CacheProvider<T> : ICacheProvider<T>
    {
        private readonly IDistributedCache cache;

        public CacheProvider(IDistributedCache cache)
        {
            this.cache = cache;
        }

        public async Task<T?> GetAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) 
            {
                return default(T);
            }

            var result = await cache.GetStringAsync(key);

            if (result == null)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(result);
        }

        public async Task SetAsync(string key, T toCache)
        {
            await cache.SetStringAsync(key, JsonSerializer.Serialize<T>(toCache));
        }

        public async Task RemoveAsync(string key) 
        {
            await cache.RemoveAsync(key);
        }
    }
}