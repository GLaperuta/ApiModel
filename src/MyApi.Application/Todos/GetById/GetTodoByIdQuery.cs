using MediatR;
using MyApi.Application.Todos.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApi.Application.Todos.GetById;

public sealed record GetTodoByIdQuery(Guid Id) : ICachedQuery<TodoDto?>
{
    public string CacheKey => $"todos:byId:{Id}";
    public TimeSpan Ttl => TimeSpan.FromMinutes(2);
}
