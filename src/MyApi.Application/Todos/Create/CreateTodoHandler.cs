using MediatR;
using MyApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using MyApi.Infrastructure.Repositories.Interfaces;
using MyApi.Infrastructure.Caching.Interfaces;

namespace MyApi.Application.Todos.Create;

public sealed class CreateTodoHandler(ITodoRepository _repo, IRedisCache _cache) : IRequestHandler<CreateTodoCommand, TodoDto>
{
    //private readonly ITodoRepository _repo;

    //private readonly IRedisCache _cache;


    //public CreateTodoHandler(ITodoRepository repo, IRedisCache cache ) 
    //{
    //    _repo = repo;
    //    _cache = cache;
    //}

    public async Task<TodoDto> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        var entity = new TodoItem(request.Title);
        await _repo.AddAsync(entity, cancellationToken);
        await _repo.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync("todos:all", cancellationToken);

        return new TodoDto(entity.Id, entity.Title, entity.Done, entity.Version);
    }
}