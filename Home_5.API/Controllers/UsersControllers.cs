using Home_5.API.Interfaces;
using Home_5.API.Requests.Users;
using Home_5.API.Services;
using Home_5.BLL.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Home_5.API.Controllers;

[ApiController]
[Route("v1/users")]
public class UsersControllers : ControllerBase
{
    private readonly IUsersService _usersService;

    public UsersControllers(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllUsersFilter filter)
    {
        if (filter == null)
        {
            filter = new GetAllUsersFilter();
        }
        
        var result = await _usersService.GetAllAsync(filter);
        return Ok(result);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var user = await _usersService.GetByIdAsync(id);
        return Ok(user);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var user = await _usersService.CreateAsync(request);
        return Ok(user);
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserRequest request)
    {
        var user = await _usersService.UpdateAsync(id, request);
        return Ok(user);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var user = await _usersService.DeleteAsync(id);
        return Ok(user);
    }
    
    [HttpPost("{id:int}/deactivate")]
    public async Task<IActionResult> Deactivate([FromRoute] int id)
    {
        var user = await _usersService.DeactivateAsync(id);
        return Ok(user);
    }
    
    [HttpGet("most-subscriptions")]
    public async Task<IActionResult> GetUserWithMostSubscriptions()
    {
        var user = await _usersService.GetUserWithMostSubscriptionsAsync();
        return Ok(user);
    }
    
    
}