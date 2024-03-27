using KFU.Core.Interfaces.Cache;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;

namespace KFU.Data.Cache
{
    public class RedisCacheService : ICacheService 
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetDataAsync<T>(string key)
        {
            var value = await _cache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            return default;
        }

        public async Task<object> RemoveDataAsync(string key)
        {
            var value = await _cache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(value))
            {
                 await _cache.RemoveAsync(value);
                return true;
            }
            return false;
        }

        public async Task<bool> SetDataAsync<T>(string key, T value, DateTimeOffset expirationTime)
        {
           // TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
             await _cache.SetStringAsync(key, JsonConvert.SerializeObject(value));
            return true;
        }
        
    }
}
