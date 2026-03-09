using DotNetEnv;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyApi.Api.Middlewares;
using MyApi.Application.Behaviors;
using MyApi.Infrastructure.Caching;
using MyApi.Infrastructure.Caching.Interfaces;
using MyApi.Infrastructure.Database;
using MyApi.Infrastructure.Repositories;
using MyApi.Infrastructure.Repositories.Interfaces;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container.

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


//CONFIGURAÇÕES DE CONEXÃO COM POSTGRES E REDIS A PARTIR DE VARIÁVEIS DE AMBIENTE
Env.Load();

var postgresHost = Environment.GetEnvironmentVariable("POSTGRES_HOST");
var postgresPort = Environment.GetEnvironmentVariable("POSTGRES_PORT");
var postgresDb = Environment.GetEnvironmentVariable("POSTGRES_DB");
var postgresUser = Environment.GetEnvironmentVariable("POSTGRES_USER");
var postgresPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

var redisHost = Environment.GetEnvironmentVariable("REDIS_HOST");
var redisPort = Environment.GetEnvironmentVariable("REDIS_PORT");

var postgresConnectionString =
    $"Host={postgresHost};Port={postgresPort};Database={postgresDb};Username={postgresUser};Password={postgresPassword}";

var redisConnectionString = $"{redisHost}:{redisPort}";

//EF
builder.Services.AddDbContext<DatabaseContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

//Repo
builder.Services.AddScoped<ITodoRepository, TodoRepository>();

//Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!));
builder.Services.AddScoped<IRedisCache, RedisCache>();

//MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(typeof(MyApi.Application.Todos.Create.CreateTodoCommand).Assembly));


//Pipeline
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

//Middlewares
builder.Services.AddScoped<ExceptionMiddleware>();
builder.Services.AddScoped<CorrelationIdMiddleware>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Doc"));
}

//APLICA MIGRATIONS AUTOMATICAMENTE NA INICIALIZAÇÃO
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    db.Database.Migrate(); //cria/atualiza tabelas conforme migrations :D
}

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

//Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program 
{
    protected Program() { }
}
