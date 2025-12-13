using Home_5.BLL.Enums;

namespace Home_5.BLL.Filters;

public class GetAllSubscriptionsFilter : PaginationFilter
{
    public SubscriptionsEnum? Type { get; set; }
    public bool? IsExpired { get; set; }
    public bool? IsCanceled { get; set; }
    public decimal? PriceGreaterThan  { get; set; }
}