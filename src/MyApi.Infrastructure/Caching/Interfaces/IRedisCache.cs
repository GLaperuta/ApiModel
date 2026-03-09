using System;
using System.Collections.Generic;
using System.Text;

namespace MyApi.Infrastructure.Caching.Interfaces
{
    public interface IRedisCache
    {
        Task<T?> GetAsync<T>(string key, CancellationToken ct);
        Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken ct);
        Task RemoveAsync(string key, CancellationToken ct);
    }
}
