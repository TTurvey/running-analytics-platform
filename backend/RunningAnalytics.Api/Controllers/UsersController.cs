using Microsoft.AspNetCore.Mvc;
using RunningAnalytics.Application.Services;
using RunningAnalytics.Application.DTOs;

namespace RunningAnalytics.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserResponse>>> GetAll()
    {
        var users = await _usersService.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserResponse>> Get(Guid id)
    {
        var user = await _usersService.GetByIdAsync(id);
        if (user is null)
        {
            return NotFound("User with the given Id was not found.");
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserResponse>> Add(CreateUserRequest request)
    {
        var createdUser = await _usersService.AddAsync(request);
        return CreatedAtAction(nameof(Get), new { id = createdUser.Id }, createdUser);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, UpdateUserRequest request)
    {
        var updated = await _usersService.UpdateAsync(id, request);
        return updated ? NoContent() : NotFound("User with the given Id was not found.");
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var deleted = await _usersService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound("User with the given Id was not found.");
    }
}
