using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyApi.Application.Todos;
using MyApi.Application.Todos.Create;
using MyApi.Application.Todos.GetAll;
using MyApi.Application.Todos.GetById;

namespace MyApi.Api.Controllers
{
    [ApiController]
    [Route("api/todos")]
    public sealed class TodosController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TodosController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public Task<TodoDto> Create([FromBody] CreateTodoCommand cmd, CancellationToken ct)
            => _mediator.Send(cmd, ct);

        [HttpGet("{id:guid}")]
        public Task<TodoDto?> GetById(Guid id, CancellationToken ct)
            => _mediator.Send(new GetTodoByIdQuery(id), ct);

        [HttpGet]
        public Task<List<TodoDto>> GetAll(CancellationToken ct)
            => _mediator.Send(new GetAllTodosQuery(), ct);
    }
}
