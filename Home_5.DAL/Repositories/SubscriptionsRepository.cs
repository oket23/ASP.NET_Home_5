using Home_5.BLL.Filters;
using Home_5.BLL.Interfaces.Repositories;
using Home_5.BLL.Models;
using Home_5.DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Home_5.DAL.Repositories;

public class SubscriptionsRepository : BaseRepository<Subscription>, ISubscriptionsRepository
{
    public SubscriptionsRepository(HomeContext context) : base(context)
    {
    }

    public async Task<ResponseWrapper<Subscription>> GetAll(GetAllSubscriptionsFilter? filter)
    {
        filter ??= new GetAllSubscriptionsFilter();
        
        var query = Set.Include(s => s.User).AsQueryable();
        
        if (filter.Type.HasValue)
        {
            query = query.Where(s => s.Type == filter.Type.Value);
        }
        
        if (filter.IsExpired == true)
        {
            query = query.Where(s => s.EndDate < DateTime.Now);
        }

        if (filter.IsCanceled == true)
        {
            query = query.Where(s => s.IsCanceled);
        }
        
        if (filter.PriceGreaterThan.HasValue)
        {
            query = query.Where(s => s.Price > filter.PriceGreaterThan.Value);
        }

        var totalCount = await query.CountAsync();
        
        var items = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return new ResponseWrapper<Subscription>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize
        };
    }

    public async Task<Subscription?> Deactivate(int subscriptionId)
    {
        var subscription = await GetById(subscriptionId);
        if (subscription is null)
        {
            return null;
        }
        
        subscription.IsCanceled = true;
        await Context.SaveChangesAsync();
        return subscription;
    }

    public async Task<Dictionary<string, int>> GetCountByType()
    {
        return await Set
            .GroupBy(s => s.Type)
            .Select(g => new { Type = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Type.ToString(), x => x.Count);
    }
    
    public async Task<Dictionary<string, int>> GetUsersCountBySubscriptionType()
    {
        return await Set
            .GroupBy(s => s.Type)
            .ToDictionaryAsync(
                g => g.Key.ToString(),
                g => g.Select(x => x.UserId).Distinct().Count()
            );
    }
}