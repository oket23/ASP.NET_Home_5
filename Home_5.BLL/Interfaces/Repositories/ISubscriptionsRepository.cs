using Home_5.BLL.Filters;
using Home_5.BLL.Models;

namespace Home_5.BLL.Interfaces.Repositories;

public interface ISubscriptionsRepository
{
    Task<ResponseWrapper<Subscription>> GetAll(GetAllSubscriptionsFilter? filter);
    Task<Subscription?> GetById(int subscriptionId);
    Task<Subscription> Create(Subscription subscription);
    Task<Subscription?> Update(Subscription subscription);
    Task<Subscription?> Delete(int subscriptionId);
    Task<Subscription?> Deactivate(int subscriptionId);
    Task<Dictionary<string, int>> GetCountByType();
    Task<Dictionary<string, int>> GetUsersCountBySubscriptionType();
}