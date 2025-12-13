using Home_5.API.Extensions;
using Home_5.API.Interfaces;
using Home_5.API.Requests.Subscriptions;
using Home_5.API.Responses;
using Home_5.API.Validations;
using Home_5.BLL.Filters;
using Home_5.BLL.Interfaces.Repositories;
using Home_5.BLL.Models;

namespace Home_5.API.Services;

public class SubscriptionsService : ISubscriptionsService
{
    private readonly ISubscriptionsRepository _subscriptionsRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly ILogger<SubscriptionsService> _logger;

    public SubscriptionsService(
        ISubscriptionsRepository subscriptionsRepository, 
        IUsersRepository usersRepository,
        ILogger<SubscriptionsService> logger)
    {
        _subscriptionsRepository = subscriptionsRepository;
        _usersRepository = usersRepository;
        _logger = logger;
    }

    public async Task<ResponseWrapper<SubscriptionResponse>> GetAllAsync(GetAllSubscriptionsFilter? filter)
    {
        _logger.LogInformation("Fetching all subscriptions with filter options");

        filter ??= new GetAllSubscriptionsFilter();
    
        var result = await _subscriptionsRepository.GetAll(filter);
        
        _logger.LogInformation("Successfully fetched {Count} subscriptions", result.Items.Count());

        var responseItems = result.Items.Select(s => s.ToResponse()).ToList();

        return new ResponseWrapper<SubscriptionResponse>
        {
            Items = responseItems,
            TotalCount = result.TotalCount, 
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
    
    public async Task<SubscriptionResponse> GetByIdAsync(int id)
    {
        _logger.LogInformation("Fetching subscription with ID: {Id}", id);

        SubscriptionValidator.IsValidId(id);

        var subscription = await _subscriptionsRepository.GetById(id);
        
        if (subscription == null)
        {
            _logger.LogWarning("Subscription with ID: {Id} not found", id);
            throw new KeyNotFoundException($"Subscription with ID {id} not found.");
        }

        return subscription.ToResponse();
    }
    
    public async Task<SubscriptionResponse> CreateAsync(CreateSubscriptionRequest request) 
    {
        _logger.LogInformation("Creating subscription '{Title}' for User ID: {UserId}", request.Title, request.UserId);

        SubscriptionValidator.ValidateSubscription(
            request.Title, 
            request.Price, 
            request.StartDate, 
            request.EndDate, 
            request.Type
        );
        
        var user = await _usersRepository.GetById(request.UserId);

        if (user == null)
        {
            _logger.LogWarning("Creation failed. User with ID: {UserId} not found", request.UserId);
            throw new KeyNotFoundException($"User with ID {request.UserId} not found.");
        }
        
        var subscription = new Subscription()
        {
            Title = request.Title,
            Price = request.Price,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Type = request.Type,
            UserId = user.Id, 
            IsCanceled = false,
            IsDeleted = false
        };

        try
        {
            var createdSubscription = await _subscriptionsRepository.Create(subscription);
            _logger.LogInformation("Subscription created successfully with ID: {Id}", createdSubscription.Id);
            
            return createdSubscription.ToResponse();
        }
        catch (Exception ex)
        {
            if (ex.InnerException?.Message.Contains("duplicate") == true || ex.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning(ex, "Failed to create subscription. Duplicate constraint violation");
                throw new ArgumentException($"A similar subscription already exists.");
            }
            
            _logger.LogError(ex, "Error creating subscription in database");
            throw;
        }
    }

    public async Task<SubscriptionResponse> UpdateAsync(int subsId, UpdateSubscriptionRequest request)
    {
        _logger.LogInformation("Attempting to update subscription with ID: {Id}", subsId);

        SubscriptionValidator.IsValidId(subsId);
        
        var subscription = await _subscriptionsRepository.GetById(subsId);
        
        if (subscription == null)
        {
            _logger.LogWarning("Update failed. Subscription with ID: {Id} not found", subsId);
            throw new KeyNotFoundException($"Subscription with ID {subsId} not found.");
        }
        
        var effectiveTitle = request.Title ?? subscription.Title;
        var effectivePrice = request.Price ?? subscription.Price;
        var effectiveStartDate = request.StartDate ?? subscription.StartDate;
        var effectiveEndDate = request.EndDate ?? subscription.EndDate;
        var effectiveType = request.Type ?? subscription.Type;
        
        SubscriptionValidator.ValidateSubscription(
            effectiveTitle,
            effectivePrice,
            effectiveStartDate,
            effectiveEndDate,
            effectiveType
        );
        
        subscription.Title = effectiveTitle;
        subscription.Price = effectivePrice;
        subscription.StartDate = effectiveStartDate;
        subscription.EndDate = effectiveEndDate;
        subscription.Type = effectiveType;
        
        try 
        {
            var updatedSubscription = await _subscriptionsRepository.Update(subscription);
            
            if (updatedSubscription == null)
            {
                 throw new Exception("An error occurred while updating the subscription in the database.");
            }

            _logger.LogInformation("Subscription with ID: {Id} updated successfully", subsId);
            return updatedSubscription.ToResponse();
        }
        catch (Exception ex)
        {
            if (ex.InnerException?.Message.Contains("duplicate") == true || 
                ex.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning(ex, "Failed to update subscription {Id}. Duplicate constraint violation", subsId);
                throw new ArgumentException($"Subscription update failed due to duplicate data.");
            }
            
            _logger.LogError(ex, "Error updating subscription {Id} in database", subsId);
            throw;
        }
    }
    
    public async Task<SubscriptionResponse> DeactivateAsync(int id)
    {
        _logger.LogInformation("Attempting to deactivate (cancel) subscription with ID: {Id}", id);

        SubscriptionValidator.IsValidId(id);
        
        var result = await _subscriptionsRepository.Deactivate(id);

        if (result == null)
        {
            _logger.LogWarning("Deactivation failed. Subscription with ID: {Id} not found", id);
            throw new KeyNotFoundException($"Subscription with ID {id} not found.");
        }
        
        _logger.LogInformation("Subscription with ID: {Id} deactivated successfully", id);
        return result.ToResponse();
    }

    public async Task<SubscriptionResponse> DeleteAsync(int id)
    {
        _logger.LogInformation("Attempting to delete subscription with ID: {Id}", id);

        SubscriptionValidator.IsValidId(id);
        
        var result = await _subscriptionsRepository.Delete(id);
        
        if (result == null)
        {
            _logger.LogWarning("Delete failed. Subscription with ID: {Id} not found", id);
            throw new KeyNotFoundException($"Subscription with ID {id} not found.");
        }
        
        _logger.LogInformation("Subscription with ID: {Id} deleted successfully", id);
        return result.ToResponse();
    }
    
    public async Task<Dictionary<string, int>> GetCountByTypeAsync()
    {
        _logger.LogInformation("Calculating subscriptions count by type");
        
        var result = await _subscriptionsRepository.GetCountByType();
        
        _logger.LogInformation("Calculated counts for {Count} types", result.Count);
        
        return result;
    }
    
    public async Task<Dictionary<string, int>> GetUsersCountBySubscriptionTypeAsync()
    {
        _logger.LogInformation("Calculating unique users count by subscription type");

        var result = await _subscriptionsRepository.GetUsersCountBySubscriptionType();
        
        _logger.LogInformation("Calculated user counts for {Count} types", result.Count);

        return result;
    }
}