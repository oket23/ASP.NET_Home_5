namespace Home_5.BLL.Models;

public class User : BaseEntity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public DateTime BirthDate { get; set; }
    public ICollection<Subscription> Subscriptions { get; set; } = new  HashSet<Subscription>();
}