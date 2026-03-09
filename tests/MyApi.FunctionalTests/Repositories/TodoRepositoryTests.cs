using Microsoft.EntityFrameworkCore;
using MyApi.Domain.Models;
using MyApi.Infrastructure.Database;
using MyApi.Infrastructure.Repositories;
using Xunit;

namespace MyApi.FunctionalTests.Repositories;

public sealed class TodoRepositoryTests
{
    [Fact]
    public async Task GetByIdAsync_ShouldReturnItem_WhenExists()
    {
        var dbName = $"Db_{Guid.NewGuid():N}";
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        await using var db = new DatabaseContext(options);
        var repo = new TodoRepository(db);

        var todo = new TodoItem("teste");
        await repo.AddAsync(todo, CancellationToken.None);
        await repo.SaveChangesAsync(CancellationToken.None);

        var result = await repo.GetByIdAsync(todo.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(todo.Id, result!.Id);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnItems()
    {
        var dbName = $"Db_{Guid.NewGuid():N}";
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        await using var db = new DatabaseContext(options);
        var repo = new TodoRepository(db);

        await repo.AddAsync(new TodoItem("a"), CancellationToken.None);
        await repo.AddAsync(new TodoItem("b"), CancellationToken.None);
        await repo.SaveChangesAsync(CancellationToken.None);

        var result = await repo.GetAllAsync(CancellationToken.None);

        Assert.Equal(2, result.Count);
    }
}