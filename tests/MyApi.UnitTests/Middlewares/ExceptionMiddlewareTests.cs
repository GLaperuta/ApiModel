using System.Text.Json;
using Microsoft.AspNetCore.Http;
using MyApi.Api.Middlewares;
using Xunit;

namespace MyApi.UnitTests.Middlewares;

public sealed class ExceptionMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_ShouldReturnBadRequest_WhenExceptionIsThrown()
    {
        var middleware = new ExceptionMiddleware();
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        await middleware.InvokeAsync(context, _ => throw new InvalidOperationException("erro de teste"));

        context.Response.Body.Position = 0;
        var body = await new StreamReader(context.Response.Body).ReadToEndAsync();

        Assert.Equal(400, context.Response.StatusCode);
        Assert.Equal("application/problem+json", context.Response.ContentType);
        Assert.Contains("erro de teste", body);

        using var doc = JsonDocument.Parse(body);
        Assert.Equal("Request failed", doc.RootElement.GetProperty("title").GetString());
        Assert.Equal(400, doc.RootElement.GetProperty("status").GetInt32());
    }

    [Fact]
    public async Task InvokeAsync_ShouldCallNext_WhenNoExceptionOccurs()
    {
        var middleware = new ExceptionMiddleware();
        var context = new DefaultHttpContext();

        var called = false;

        await middleware.InvokeAsync(context, _ =>
        {
            called = true;
            return Task.CompletedTask;
        });

        Assert.True(called);
    }
}