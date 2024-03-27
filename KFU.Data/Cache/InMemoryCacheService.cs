using KFU.Core.Interfaces.Cache;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFU.Data.Cache
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public InMemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public  Task<T> GetDataAsync<T>(string key)
        {
            var value =  _cache.Get<T>(key);
            return value == null ? default : Task.FromResult(value);
        }

        public Task<object> RemoveDataAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetDataAsync<T>(string key, T value, DateTimeOffset expirationTime)
        {
            TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            _cache.Set(key, value, expiryTime);
            return Task.FromResult(true);
        }
    }
}
