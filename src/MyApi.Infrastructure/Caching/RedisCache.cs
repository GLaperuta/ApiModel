using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using MyApi.Infrastructure.Caching.Interfaces;

namespace MyApi.Infrastructure.Caching
{
    public sealed class RedisCache : IRedisCache
    {
        private readonly IDatabase _db;
        public RedisCache(IConnectionMultiplexer mux) => _db = mux.GetDatabase();

        public async Task<T?> GetAsync<T>(string key, CancellationToken ct)
        {
            var value = await _db.StringGetAsync(key);
            if (!value.HasValue) return default;
            return JsonSerializer.Deserialize<T>(value.ToString()!);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken ct)
            => _db.StringSetAsync(key, JsonSerializer.Serialize(value), ttl);

        public Task RemoveAsync(string key, CancellationToken ct)
            => _db.KeyDeleteAsync(key);
    }
}
