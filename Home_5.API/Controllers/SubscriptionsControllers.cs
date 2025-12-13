using Home_5.API.Interfaces;
using Home_5.API.Requests.Subscriptions;
using Home_5.BLL.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Home_5.API.Controllers;

[ApiController]
[Route("v1/subscriptions")]
public class SubscriptionsControllers : ControllerBase
{
    private readonly ISubscriptionsService _subscriptionsService;

    public SubscriptionsControllers(ISubscriptionsService subscriptionsService)
    {
        _subscriptionsService = subscriptionsService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllSubscriptionsFilter? filter)
    {
        if (filter == null)
        {
            filter = new GetAllSubscriptionsFilter();
        }
        
        var result = await _subscriptionsService.GetAllAsync(filter);
        return Ok(result);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var subs = await _subscriptionsService.GetByIdAsync(id);
        return Ok(subs);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSubscriptionRequest request)
    {
        var subs = await _subscriptionsService.CreateAsync(request);
        return Ok(subs);
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateSubscriptionRequest request)
    {
        var subs = await _subscriptionsService.UpdateAsync(id, request);
        return Ok(subs);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var subs = await _subscriptionsService.DeleteAsync(id);
        return Ok(subs);
    }
    
    [HttpPost("{id:int}/deactivate")]
    public async Task<IActionResult> Deactivate([FromRoute] int id)
    {
        var subs = await _subscriptionsService.DeactivateAsync(id);
        return Ok(subs);
    }
    
    [HttpGet("count-by-type")]
    public async Task<IActionResult> GetCountByType()
    {
        var result = await _subscriptionsService.GetCountByTypeAsync();
        return Ok(result);
    }
    
    [HttpGet("users-count-by-type")]
    public async Task<IActionResult> GetUsersCountBySubscriptionType()
    {
        var result = await _subscriptionsService.GetUsersCountBySubscriptionTypeAsync();
        return Ok(result);
    }
}