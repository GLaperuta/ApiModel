using MediatR;
using MyApi.Application.Behaviors;
using MyApi.Application.Todos;
using MyApi.Application.Todos.Interfaces;
using MyApi.FunctionalTests.Fakes;
using Xunit;

namespace MyApi.UnitTests.Behaviors;

public sealed class CachingBehaviorTests
{
    private sealed record NonCachedRequest : IRequest<string>;

    private sealed record CachedRequest(string Key) : ICachedQuery<string>
    {
        public string CacheKey => Key;
        public TimeSpan Ttl => TimeSpan.FromMinutes(1);
    }

    [Fact]
    public async Task Handle_ShouldCallNext_WhenRequestIsNotCachedQuery()
    {
        var cache = new FakeCache();
        var behavior = new CachingBehavior<NonCachedRequest, string>(cache);

        var called = false;

        var result = await behavior.Handle(
            new NonCachedRequest(),
            (next) =>
            {
                called = true;
                return Task.FromResult("ok");
            },
            CancellationToken.None
        );

        Assert.True(called);
        Assert.Equal("ok", result);
    }

    [Fact]
    public async Task Handle_ShouldReturnCachedValue_WhenCacheHit()
    {
        var cache = new FakeCache();
        await cache.SetAsync("todos:1", "valor-em-cache", TimeSpan.FromMinutes(1), CancellationToken.None);

        var behavior = new CachingBehavior<CachedRequest, string>(cache);

        var result = await behavior.Handle(
            new CachedRequest("todos:1"),
            (next) => Task.FromResult("valor-do-next"),
            CancellationToken.None);

        Assert.Equal("valor-em-cache", result);
    }

    [Fact]
    public async Task Handle_ShouldCallNextAndStoreValue_WhenCacheMiss()
    {
        var cache = new FakeCache();
        var behavior = new CachingBehavior<CachedRequest, string>(cache);

        var called = false;

        var result = await behavior.Handle(
            new CachedRequest("todos:2"),
            (next) =>
            {
                called = true;
                return Task.FromResult("novo-valor");
            },
            CancellationToken.None);

        var saved = await cache.GetAsync<string>("todos:2", CancellationToken.None);

        Assert.True(called);
        Assert.Equal("novo-valor", result);
        Assert.Equal("novo-valor", saved);
    }
}