using MediatR;
using MyApi.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApi.Application.Todos.GetAll;

public sealed class GetAllTodosHandler(ITodoRepository _repo) : IRequestHandler<GetAllTodosQuery, List<TodoDto>>
{
    //private readonly ITodoRepository _repo;
    //public GetAllTodosHandler(ITodoRepository repo) => _repo = repo;

    public async Task<List<TodoDto>> Handle(GetAllTodosQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.GetAllAsync(cancellationToken);
        //return items.Select(x => new TodoDto(x.Id, x.Title, x.Done, x.Version)).ToList();
        return [..items.Select(x => new TodoDto(x.Id, x.Title, x.Done, x.Version))];
    }
}
