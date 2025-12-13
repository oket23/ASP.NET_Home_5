using Home_5.API.Requests.Users;
using Home_5.API.Responses;
using Home_5.BLL.Filters;
using Home_5.BLL.Models;

namespace Home_5.API.Interfaces;

public interface IUsersService
{
    Task<ResponseWrapper<UserResponse>> GetAllAsync(GetAllUsersFilter? filter);
    Task<UserResponse> GetByIdAsync(int id);
    Task<UserResponse> CreateAsync(CreateUserRequest request);
    Task<UserResponse> UpdateAsync(int userId, UpdateUserRequest request);
    Task<UserResponse> DeleteAsync(int id);
    Task<UserResponse> DeactivateAsync(int id);
    Task<UserResponse> GetUserWithMostSubscriptionsAsync();
}