using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Home_5.API.Middleware;

public class MetricsMiddleware : IMiddleware
{
    private readonly ILogger<MetricsMiddleware> _logger;

    public MetricsMiddleware(ILogger<MetricsMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopwatch = Stopwatch.StartNew();
        
        var originalBodyStream = context.Response.Body;
        
        using var memoryStream = new MemoryStream();
        
        context.Response.Body = memoryStream;

        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();
            var elapsed = stopwatch.ElapsedMilliseconds;

            _logger.LogInformation("Request {RequestMethod} {RequestPath} took {Elapsed} ms", context.Request.Method, context.Request.Path, elapsed);
            
            memoryStream.Position = 0;
            var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();
    
            JsonNode? data = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(responseBody))
                {
                    data = JsonNode.Parse(responseBody);
                }
            }
            catch
            {
                data = responseBody;
            }
            
            var wrappedResponse = new
            {
                Data = data,                      
                ExecutionTime = $"{elapsed} ms",   
                Author = "Copyright by Oleh"  
            };
            
            var newJson = JsonSerializer.Serialize(wrappedResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true 
            });
            
            context.Response.Body = originalBodyStream;
            
            var newBytes = System.Text.Encoding.UTF8.GetBytes(newJson);
            context.Response.ContentLength = newBytes.Length;
            context.Response.ContentType = "application/json";
            
            await context.Response.Body.WriteAsync(newBytes);
        }
    }
}