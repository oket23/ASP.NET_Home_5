using Home_5.BLL.Models;
using Microsoft.EntityFrameworkCore;

namespace Home_5.DAL.Repositories.Base;

public abstract class BaseRepository<T> where T: BaseEntity
{
    protected readonly HomeContext Context;
    protected DbSet<T> Set => Context.Set<T>();

    protected BaseRepository(HomeContext context)
    {
        Context = context;
    }

    public virtual async Task<T?> GetById(int id)
    {
        return await Set.FirstOrDefaultAsync(e => e.Id == id);
    }
    
    public virtual async Task<T> Create(T entity)
    {
        Set.Add(entity);
        
        await Context.SaveChangesAsync();
        return entity;
    }
    
    public virtual async Task<T?> Update(T entity)
    {
        Set.Update(entity);
        
        await Context.SaveChangesAsync();
        return entity;
    }
    
    public virtual async Task<T?> Delete(int entityId)
    {
        var entity = await GetById(entityId);
        if (entity is null)
        {
            return null;
        }

        Set.Remove(entity);
        await Context.SaveChangesAsync();
        return entity;
    }
}