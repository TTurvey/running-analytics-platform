using Microsoft.AspNetCore.Mvc;
using RunningAnalytics.Api.Models;
using RunningAnalytics.Api.Services;

namespace RunningAnalytics.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(IUsersService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAll()
    {
        var users = await service.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<User>> Get(Guid id)
    {
        var user = await service.GetByIdAsync(id);
        if (user is null)
        {
            return NotFound("User with the given Id was not found.");
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<User>> Add(User user)
    {
        await service.AddAsync(user);

        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }
}
