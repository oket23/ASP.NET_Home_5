using System.Text.Json;
using Home_5.API.Responses.Base;

namespace Home_5.API.Middleware;

public class GlobalExceptionHandler : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _env;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unhandled exception occurred: {Message}", e.Message);
            
            context.Response.StatusCode = e switch
            {
                ArgumentException => 400,        
                KeyNotFoundException => 404,
                UnauthorizedAccessException => 403,
                _ => 500                      
            };

            context.Response.ContentType = "application/json";

            var errorResponse = new ResponseError
            {
                Message = e.Message,
                StackTrace = _env.IsDevelopment() ? e.StackTrace : null 
            };

            var errorJson = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(errorJson);
        }
    }
}
