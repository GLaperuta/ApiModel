using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using MyApi.Domain.Models;
using MyApi.Infrastructure.Database;
using MyApi.Infrastructure.Repositories.Interfaces;

namespace MyApi.Infrastructure.Repositories;

public sealed class TodoRepository : ITodoRepository
{
    private readonly DatabaseContext _db;
    public TodoRepository(DatabaseContext db) => _db = db;

    public Task AddAsync(TodoItem item, CancellationToken ct) => _db.Todos.AddAsync(item, ct).AsTask();

    public Task<TodoItem?> GetByIdAsync(Guid id, CancellationToken ct) =>
        _db.Todos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<List<TodoItem>> GetAllAsync(CancellationToken ct) =>
        _db.Todos.AsNoTracking().OrderByDescending(x => x.Id).ToListAsync(ct);

    public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
}
