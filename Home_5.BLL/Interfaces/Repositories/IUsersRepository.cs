using Home_5.BLL.Filters;
using Home_5.BLL.Models;

namespace Home_5.BLL.Interfaces.Repositories;

public interface IUsersRepository
{
    Task<ResponseWrapper<User>> GetAll(GetAllUsersFilter? filter);
    Task<User?> GetById(int userId);
    Task<User> Create(User user);
    Task<User?> Update(User user);
    Task<User?> Delete(int userId);
    Task<User?> GetUserWithMostSubscriptions();
    Task<User?> Deactivate(int userId);
}