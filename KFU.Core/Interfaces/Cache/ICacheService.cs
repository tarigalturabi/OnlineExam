using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFU.Core.Interfaces.Cache
{
    public interface ICacheService
    {

        Task<T> GetDataAsync<T>(string key);     
        Task<bool> SetDataAsync<T>(string key, T value, DateTimeOffset expirationTime);      
        Task<object> RemoveDataAsync(string key);
    }
}
