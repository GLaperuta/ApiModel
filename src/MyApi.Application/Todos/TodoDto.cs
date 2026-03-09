using System;
using System.Collections.Generic;
using System.Text;

namespace MyApi.Application.Todos;

public sealed record TodoDto(Guid Id, string Title, bool Done, Guid Version);
