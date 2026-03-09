using Microsoft.EntityFrameworkCore;
using MyApi.Application.Todos.Create;
using MyApi.FunctionalTests.Fakes;
using MyApi.Infrastructure.Database;
using MyApi.Infrastructure.Repositories;
using Xunit;

namespace MyApi.FunctionalTests;

public sealed class CreateTodoHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateTodo_AndInvalidateCache()
    {
        // Arrange
        var (db, repo, cache, handler) = BuildSut(); // monta ambiente :D, menos codigo e menos repetitivo.
        await using (db)
        {
            await db.Database.EnsureCreatedAsync();

            var command = new CreateTodoCommand("Estudar DDD");

            //coloca algo no cache antes
            await cache.SetAsync("todos:all", new object(), TimeSpan.FromMinutes(1), CancellationToken.None);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            //verifica se salvou no banco
            var saved = await db.Todos.FirstOrDefaultAsync(x => x.Id == result.Id);
            Assert.NotNull(saved);
            Assert.Equal("Estudar DDD", saved!.Title);

            //Assert
            //verifica se o cache foi invalidado
            var stillThere = await cache.GetAsync<object>("todos:all", CancellationToken.None);
            Assert.Null(stillThere);
        }
    }

    [Fact]
    public async Task Handle_ShouldCreateTodo_AndPersistInDatabase()
    {
        //Arrange
        var (db, repo, cache, handler) = BuildSut();
        await using (db)
        {
            await db.Database.EnsureCreatedAsync();

            var command = new CreateTodoCommand("Estudar CQRS");

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            //valida retorno do handler
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal("Estudar CQRS", result.Title);
            Assert.False(result.Done);
            Assert.NotEqual(Guid.Empty, result.Version);

            //Assert
            //valida que persistiu no DB (não só retornou DTO)
            var saved = await db.Todos.FirstOrDefaultAsync(x => x.Id == result.Id);
            Assert.NotNull(saved);
            Assert.Equal("Estudar CQRS", saved!.Title);
            Assert.False(saved.Done);
            Assert.Equal(result.Version, saved.Version);
        }
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Handle_InvalidTitle_ShouldThrow(string title)
    {
        //Arrange
        var (db, repo, cache, handler) = BuildSut();
        await using (db)
        {
            await db.Database.EnsureCreatedAsync();

            //Act + Assert :D
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await handler.Handle(new CreateTodoCommand(title), CancellationToken.None));
        }
    }

    private static (DatabaseContext db, TodoRepository repo, FakeCache cache, CreateTodoHandler handler) BuildSut()
    {
        //Cria um banco InMemory isolado por teste (evita vazamento de dados)
        var dbName = $"MyApiDb_{Guid.NewGuid():N}";

        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        var db = new DatabaseContext(options);
        var repo = new TodoRepository(db);
        var cache = new FakeCache();
        var handler = new CreateTodoHandler(repo, cache);

        return (db, repo, cache, handler);
    }
}