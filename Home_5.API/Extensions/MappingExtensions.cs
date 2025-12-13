using Home_5.API.Responses;
using Home_5.BLL.Models;

namespace Home_5.API.Extensions;

public static class MappingExtensions
{
    public static UserResponse ToResponse(this User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            BirthDate = user.BirthDate,
            Subscriptions = user.Subscriptions?.Select(s => s.ToResponse()).ToList() ?? new List<SubscriptionResponse>()
        };
    }
    public static SubscriptionResponse ToResponse(this Subscription sub)
    {
        return new SubscriptionResponse
        {
            Id = sub.Id,
            Title = sub.Title,
            Price = sub.Price,
            StartDate = sub.StartDate,
            EndDate = sub.EndDate,
            Type = sub.Type,
            IsCanceled = sub.IsCanceled,
            UserId = sub.UserId,
        };
    }
}