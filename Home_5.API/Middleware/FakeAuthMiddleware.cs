namespace Home_5.API.Middleware;

public class FakeAuthMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Headers.TryGetValue("Token", out var token) && token == "1234")
        {
            await next(context);
            return;
        }

        if (HttpMethods.IsGet(context.Request.Method))
        {
            await next(context);
            return;
        }
        
        throw new UnauthorizedAccessException("Forbidden: You don't have permission to perform this action.");
    }
}