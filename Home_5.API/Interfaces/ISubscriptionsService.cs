using Home_5.API.Requests.Subscriptions;
using Home_5.API.Responses;
using Home_5.BLL.Filters;
using Home_5.BLL.Models;

namespace Home_5.API.Interfaces;

public interface ISubscriptionsService
{
    Task<ResponseWrapper<SubscriptionResponse>> GetAllAsync(GetAllSubscriptionsFilter? filter);
    Task<SubscriptionResponse> GetByIdAsync(int id);
    Task<SubscriptionResponse> CreateAsync(CreateSubscriptionRequest request);
    Task<SubscriptionResponse> UpdateAsync(int subsId, UpdateSubscriptionRequest request);
    Task<SubscriptionResponse> DeactivateAsync(int id);
    Task<SubscriptionResponse> DeleteAsync(int id);
    Task<Dictionary<string, int>> GetCountByTypeAsync();
    Task<Dictionary<string, int>> GetUsersCountBySubscriptionTypeAsync();
       
}
