using MyApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApi.Infrastructure.Repositories.Interfaces;

public interface ITodoRepository
{
    Task AddAsync(TodoItem item, CancellationToken ct);
    Task<TodoItem?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<List<TodoItem>> GetAllAsync(CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
