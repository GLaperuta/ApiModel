
using MyApi.Infrastructure.Caching.Interfaces;

namespace MyApi.FunctionalTests.Fakes;

public sealed class FakeCache : IRedisCache
{
    private readonly Dictionary<string, object> _store = new();

    public Task<T?> GetAsync<T>(string key, CancellationToken ct)
    {
        if (_store.TryGetValue(key, out var value) && value is T typed)
            return Task.FromResult<T?>(typed);

        return Task.FromResult<T?>(default);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken ct)
    {
        _store[key] = value!;
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key, CancellationToken ct)
    {
        _store.Remove(key);
        return Task.CompletedTask;
    }
}