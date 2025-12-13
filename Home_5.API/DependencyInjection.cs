using Home_5.API.Interfaces;
using Home_5.API.Middleware;
using Home_5.API.Services;
using Home_5.BLL.Interfaces.Repositories;
using Home_5.DAL;
using Home_5.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Home_5.API;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HomeContext>(opts => opts.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddScoped<ISubscriptionsRepository, SubscriptionsRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<ISubscriptionsService,SubscriptionsService>();
        services.AddScoped<IUsersService, UsersService>();
        
        services.AddTransient<GlobalExceptionHandler>();
        services.AddTransient<MetricsMiddleware>();
        services.AddTransient<FakeAuthMiddleware>();
        
        return services;
    }
}