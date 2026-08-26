using Microsoft.AspNetCore.Mvc;
using RunningAnalytics.Api.Services;
using RunningAnalytics.Api.dtos;

namespace RunningAnalytics.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(IUsersService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<UserResponse>>> GetAll()
    {
        var users = await service.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserResponse>> Get(Guid id)
    {
        var user = await service.GetByIdAsync(id);
        if (user is null)
        {
            return NotFound("User with the given Id was not found.");
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserResponse>> Add(CreateUserRequest request)
    {
        var createdUser = await service.AddAsync(request);
        return CreatedAtAction(nameof(Get), new { id = createdUser.Id }, createdUser);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, UpdateUserRequest request)
    {
        var updated = await service.UpdateAsync(id, request);
        return updated ? NoContent() : NotFound("User with the given Id was not found.");
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var deleted = await service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound("User with the given Id was not found.");
    }
}
