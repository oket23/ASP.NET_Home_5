using System.Text.Json.Serialization;
using Home_5.API.Middleware;
using Serilog;

namespace Home_5.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers().AddJsonOptions(opts => opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        builder.Services.AddSwaggerGen();
        
        builder.Logging.ClearProviders();
        builder.Host.UseSerilog((ctx, lc) =>
        {
            lc.ReadFrom.Configuration(ctx.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Service", ctx.Configuration["Logging:ApplicationName"])
                .WriteTo.Console(outputTemplate:"{Timestamp:HH:mm:ss} [{Level:u3}] [{Service}] {Message:lj}{NewLine}{Exception}");
        });
        
        builder.Services.AddWebApi(builder.Configuration);
        
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();
        
        app.UseSerilogRequestLogging();
        
        app.UseMiddleware<MetricsMiddleware>();
        
        app.UseMiddleware<GlobalExceptionHandler>();
        
        app.UseMiddleware<FakeAuthMiddleware>();
        
        app.MapControllers();
        app.Run();
    }
}