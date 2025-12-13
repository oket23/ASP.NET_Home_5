using Home_5.BLL.Enums;

namespace Home_5.BLL.Filters;

public class GetAllUsersFilter : PaginationFilter
{
    public string? FirstNameStartsWith { get; set; }
    public string? LastNameStartsWith { get; set; }
    public bool? HasSubscriptions { get; set; }
    public SubscriptionsEnum? HasSubscriptionType { get; set; } 
}