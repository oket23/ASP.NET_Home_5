using Home_5.BLL.Enums;

namespace Home_5.BLL.Models;

public class Subscription : BaseEntity
{
    public required string Title { get; set; }
    public decimal Price { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public SubscriptionsEnum Type { get; set; }
    public bool IsCanceled { get; set; } = false;
    public int UserId { get; set; }
    public User? User { get; set; }
}