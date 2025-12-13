using Home_5.API.Extensions;
using Home_5.API.Interfaces;
using Home_5.API.Requests.Users;
using Home_5.API.Responses;
using Home_5.API.Validations;
using Home_5.BLL.Filters;
using Home_5.BLL.Interfaces.Repositories;
using Home_5.BLL.Models;
namespace Home_5.API.Services;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;
    private readonly ILogger<UsersService> _logger;

    public UsersService(IUsersRepository usersRepository, ILogger<UsersService> logger)
    {
        _usersRepository = usersRepository;
        _logger = logger;
    }
    
    public async Task<ResponseWrapper<UserResponse>> GetAllAsync(GetAllUsersFilter? filter)
    {
        _logger.LogInformation("Fetching all users with filter options");
        
        filter ??= new GetAllUsersFilter();
        
        var result = await _usersRepository.GetAll(filter);
        
        var responseItems = result.Items.Select(u => u.ToResponse()).ToList();
        
        _logger.LogInformation("Successfully fetched {Count} users", result.Items.Count());
        
        return new ResponseWrapper<UserResponse>
        {
            Items = responseItems,
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }

    public async Task<UserResponse> GetByIdAsync(int id)
    {
        _logger.LogInformation("Fetching user with ID: {Id}", id);
        
        UserValidator.IsValidId(id);
        
        var user = await _usersRepository.GetById(id);

        if (user == null)
        {
            _logger.LogWarning("User with ID: {Id} was not found", id);
            throw new KeyNotFoundException($"User with ID {id} not found.");
        }
        
        return user.ToResponse();
    }

    public async Task<UserResponse> CreateAsync(CreateUserRequest request)
    {
        _logger.LogInformation("Creating a new user with email: {Email}", request.Email);
        
        UserValidator.ValidateUser(
            request.FirstName, 
            request.LastName, 
            request.Email, 
            request.BirthDate
        );
        
        var user = new User()
        {
            FirstName =  request.FirstName,
            LastName =  request.LastName,
            Email =  request.Email,
            BirthDate = request.BirthDate,
            IsDeleted = false
        };

        try
        {
            var createdUser = await _usersRepository.Create(user);
            _logger.LogInformation("User created successfully with ID: {Id}", createdUser.Id);
            return createdUser.ToResponse();
        }
        catch (Exception ex)
        {
            if (ex.InnerException?.Message.Contains("duplicate") == true || ex.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning(ex, "Failed to create user. Email {Email} already exists", request.Email);
                throw new ArgumentException($"User with email '{request.Email}' already exists.");
            }
            
            _logger.LogError(ex, "Error creating user in database");
            throw;
        }
    }

    public async Task<UserResponse> UpdateAsync(int userId, UpdateUserRequest request)
    {
        _logger.LogInformation("Attempting to update user with ID: {Id}", userId);

        var user = await _usersRepository.GetById(userId);
        
        if (user == null)
        {
            _logger.LogWarning("Update failed. User with ID: {Id} not found", userId);
            throw new KeyNotFoundException($"User with ID {userId} not found.");
        }
        
        var effectiveFirstName = request.FirstName ?? user.FirstName;
        var effectiveLastName = request.LastName ?? user.LastName;
        var effectiveEmail = request.Email  ?? user.Email;
        var effectiveBirthDate= request.BirthDate ?? user.BirthDate;
        
        UserValidator.ValidateUser(
            effectiveFirstName,
            effectiveLastName,
            effectiveEmail,
            effectiveBirthDate
        );
        
        user.FirstName = effectiveFirstName;
        user.LastName = effectiveLastName;
        user.Email = effectiveEmail;
        user.BirthDate = effectiveBirthDate;
        
        try
        {
            var updatedUser = await _usersRepository.Update(user);
            
            if (updatedUser == null)
            {
                 throw new Exception("An error occurred while updating the user in the database.");
            }
    
            _logger.LogInformation("User with ID: {Id} updated successfully", userId);
            return updatedUser.ToResponse();
        }
        catch (Exception ex)
        {
            // Перевірка на дублікат при оновленні (якщо змінили імейл на вже існуючий)
            if (ex.InnerException?.Message.Contains("duplicate") == true || 
                ex.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning(ex, "Failed to update user {Id}. Email {Email} already exists.", userId, effectiveEmail);
                throw new ArgumentException($"User with email '{effectiveEmail}' already exists.");
            }

            _logger.LogError(ex, "Error updating user {Id} in database.", userId);
            throw;
        }
    }
    
    public async Task<UserResponse> DeleteAsync(int id)
    {
        _logger.LogInformation("Attempting to delete user with ID: {Id}", id);
        
        UserValidator.IsValidId(id);
        
        var deletedUser = await _usersRepository.Delete(id);
        
        if (deletedUser == null)
        {
            _logger.LogWarning("Delete failed. User with ID: {Id} not found", id);
            throw new KeyNotFoundException($"User with ID {id} not found.");
        }

        _logger.LogInformation("User with ID: {Id} deleted successfully", id);
        
        return deletedUser.ToResponse();
    }
    
    public async Task<UserResponse> DeactivateAsync(int id)
    {
        _logger.LogInformation("Attempting to deactivate user with ID: {Id}", id);

        UserValidator.IsValidId(id);
        
        var deactivatedUser = await _usersRepository.Deactivate(id);
        
        if (deactivatedUser == null)
        {
             _logger.LogWarning("Deactivation failed. User with ID: {Id} not found", id);
             throw new KeyNotFoundException($"User with ID {id} not found.");
        }
        
        _logger.LogInformation("User with ID: {Id} deactivated successfully", id);

        return deactivatedUser.ToResponse();
    }
    
    public async Task<UserResponse> GetUserWithMostSubscriptionsAsync()
    {
        _logger.LogInformation("Calculating user with the most subscriptions");
        
        var result = await _usersRepository.GetUserWithMostSubscriptions();
        
        if (result == null)
        {
             _logger.LogInformation("No users with subscriptions found");
             throw new KeyNotFoundException($"Users not found.");
        }
        _logger.LogInformation("Found user ID: {ResultId} with most subscriptions", result.Id);

        return result.ToResponse();
    }
}