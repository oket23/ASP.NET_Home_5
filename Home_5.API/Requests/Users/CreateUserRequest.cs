namespace Home_5.API.Requests.Users;

public class CreateUserRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public DateTime BirthDate { get; set; }
}