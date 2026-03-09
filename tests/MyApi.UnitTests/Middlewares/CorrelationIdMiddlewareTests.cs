using Microsoft.AspNetCore.Http;
using MyApi.Api.Middlewares;
using Xunit;

namespace MyApi.UnitTests.Middlewares;

public sealed class CorrelationIdMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_ShouldGenerateHeader_WhenRequestDoesNotContainCorrelationId()
    {
        var middleware = new CorrelationIdMiddleware();
        var context = new DefaultHttpContext();

        await middleware.InvokeAsync(context, _ => Task.CompletedTask);

        Assert.True(context.Response.Headers.ContainsKey(CorrelationIdMiddleware.HeaderName));
        Assert.False(string.IsNullOrWhiteSpace(context.Response.Headers[CorrelationIdMiddleware.HeaderName]));
    }

    [Fact]
    public async Task InvokeAsync_ShouldReuseHeader_WhenRequestContainsCorrelationId()
    {
        var middleware = new CorrelationIdMiddleware();
        var context = new DefaultHttpContext();
        context.Request.Headers[CorrelationIdMiddleware.HeaderName] = "abc123";

        await middleware.InvokeAsync(context, _ => Task.CompletedTask);

        Assert.Equal("abc123", context.Response.Headers[CorrelationIdMiddleware.HeaderName].ToString());
    }

    [Fact]
    public async Task InvokeAsync_ShouldGenerateHeader_WhenRequestContainsEmptyCorrelationId()
    {
        var middleware = new CorrelationIdMiddleware();
        var context = new DefaultHttpContext();
        context.Request.Headers[CorrelationIdMiddleware.HeaderName] = "";

        await middleware.InvokeAsync(context, _ => Task.CompletedTask);

        Assert.True(context.Response.Headers.ContainsKey(CorrelationIdMiddleware.HeaderName));
        Assert.False(string.IsNullOrWhiteSpace(context.Response.Headers[CorrelationIdMiddleware.HeaderName]));
    }
}