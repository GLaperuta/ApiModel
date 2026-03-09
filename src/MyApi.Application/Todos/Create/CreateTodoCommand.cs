using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApi.Application.Todos.Create;

public sealed record CreateTodoCommand(string Title) : IRequest<TodoDto>;
