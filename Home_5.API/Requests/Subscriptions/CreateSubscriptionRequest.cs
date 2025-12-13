using Home_5.BLL.Enums;

namespace Home_5.API.Requests.Subscriptions;

public class CreateSubscriptionRequest
{
    public required string Title { get; set; }
    public decimal Price { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public SubscriptionsEnum Type { get; set; }
    public int UserId { get; set; }
}