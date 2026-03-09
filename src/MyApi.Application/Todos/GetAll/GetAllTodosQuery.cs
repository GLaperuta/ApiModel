using MediatR;
using MyApi.Application.Todos.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApi.Application.Todos.GetAll;

public sealed record GetAllTodosQuery : ICachedQuery<List<TodoDto>>
{
    public string CacheKey => "todos:all";
    public TimeSpan Ttl => TimeSpan.FromMinutes(1);
}