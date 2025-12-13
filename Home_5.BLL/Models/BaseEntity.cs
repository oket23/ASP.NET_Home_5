namespace Home_5.BLL.Models;

public class BaseEntity
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; } = false;
}