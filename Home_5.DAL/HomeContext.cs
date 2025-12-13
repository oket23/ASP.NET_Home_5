using Home_5.BLL.Models;
using Home_5.DAL.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Home_5.DAL;

public class HomeContext : DbContext
{
    public HomeContext(DbContextOptions<HomeContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
    }
}