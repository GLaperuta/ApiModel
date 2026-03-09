namespace MyApi.Api.Middlewares
{
    public sealed class CorrelationIdMiddleware : IMiddleware
    {
        public const string HeaderName = "X-Correlation-Id";

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!context.Request.Headers.TryGetValue(HeaderName, out var cid) || string.IsNullOrWhiteSpace(cid))
                cid = Guid.NewGuid().ToString("N");

            context.Response.Headers[HeaderName] = cid!;
            await next(context);
        }
    }
}
