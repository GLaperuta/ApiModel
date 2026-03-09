using MediatR;
using MyApi.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApi.Application.Todos.GetById;

public sealed class GetTodoByIdHandler(ITodoRepository _repo) : IRequestHandler<GetTodoByIdQuery, TodoDto?>
{
    //private readonly ITodoRepository _repo;
    //public GetTodoByIdHandler(ITodoRepository repo) => _repo = repo;

    public async Task<TodoDto?> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _repo.GetByIdAsync(request.Id, cancellationToken);
        return item is null ? null : new TodoDto(item.Id, item.Title, item.Done, item.Version);
    }
}
