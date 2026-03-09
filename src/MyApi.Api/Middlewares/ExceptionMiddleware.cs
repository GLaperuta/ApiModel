using System.Net;
using System.Text.Json;


namespace MyApi.Api.Middlewares
{
    public sealed class ExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try { await next(context); }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/problem+json";

                var payload = new
                {
                    title = "Request failed",
                    status = context.Response.StatusCode,
                    detail = ex.Message,
                    traceId = context.TraceIdentifier
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
            }
        }
    }
}
