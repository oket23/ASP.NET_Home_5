using Home_5.BLL.Filters;
using Home_5.BLL.Interfaces.Repositories;
using Home_5.BLL.Models;
using Home_5.DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Home_5.DAL.Repositories;

public class UsersRepository : BaseRepository<User>, IUsersRepository
{
    public UsersRepository(HomeContext context) : base(context)
    {
    }

    public async Task<ResponseWrapper<User>> GetAll(GetAllUsersFilter? filter)
    {
        filter ??= new GetAllUsersFilter(); 
        
        var query = Set.Include(u => u.Subscriptions).AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(filter.FirstNameStartsWith))
        {
            query = query.Where(u => u.FirstName.StartsWith(filter.FirstNameStartsWith));
        }
            
        if (!string.IsNullOrWhiteSpace(filter.LastNameStartsWith))
        {
            query = query.Where(u => u.LastName.StartsWith(filter.LastNameStartsWith));
        }
            
        if (filter.HasSubscriptions.HasValue && filter.HasSubscriptions.Value)
        {
            query = query.Where(u => u.Subscriptions.Any());
        }
            
        if (filter.HasSubscriptionType.HasValue)
        {
            query = query.Where(u => u.Subscriptions.Any(s => s.Type == filter.HasSubscriptionType.Value));
        }
        
        var totalCount = await query.CountAsync();
        
        var items = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return new ResponseWrapper<User>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize
        };
    }

    public async Task<User?> GetUserWithMostSubscriptions()
    {
        return await Set
            .Include(u => u.Subscriptions)
            .OrderByDescending(u => u.Subscriptions.Count) 
            .FirstOrDefaultAsync();
    }

    public async Task<User?> Deactivate(int userId)
    {
        var user = await GetById(userId);
        if (user is null)
        {
            return null;
        };
        
        user.IsDeleted = true;
        await Context.SaveChangesAsync();
        return user;
    }
}