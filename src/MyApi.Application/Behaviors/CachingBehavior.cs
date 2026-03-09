using MediatR;
using MyApi.Application.Todos.Interfaces;
using MyApi.Infrastructure.Caching.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApi.Application.Behaviors
{
    public sealed class CachingBehavior<TRequest, TResponse>(IRedisCache _cache) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    {
        //private readonly IRedisCache _cache;

        //public CachingBehavior(IRedisCache cache) => _cache = cache;

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (request is not ICachedQuery<TResponse> cached)
            {
                return await next(cancellationToken);
            }
                

            var hit = await _cache.GetAsync<TResponse>(cached.CacheKey, cancellationToken);
            if (hit is not null)
                return hit;

            var result = await next(cancellationToken);
            await _cache.SetAsync(cached.CacheKey, result, cached.Ttl, cancellationToken);
            return result;
        }
    }
}
