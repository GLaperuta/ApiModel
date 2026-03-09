using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApi.Application.Todos.Interfaces
{
    public interface ICachedQuery<out TResponse> : IRequest<TResponse>
    {
        string CacheKey { get; }
        TimeSpan Ttl { get; }
    }
}
