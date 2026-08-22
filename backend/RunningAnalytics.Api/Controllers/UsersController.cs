using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunningAnalytics.Api.Data;
using RunningAnalytics.Api.Models;

namespace RunningAnalytics.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly RunningAnalyticsDbContext _dbContext;

    public UsersController(RunningAnalyticsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await _dbContext.Users
            .OrderBy(u => u.CreatedAt)
            .ToListAsync();

        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<User>> GetUser(Guid id)
    {
        var user = await _dbContext.Users.FindAsync(id);

        return user is null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(UserCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest("Email is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest("Name is required.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = request.PasswordHash,
            Name = request.Name,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }
}

public record UserCreateRequest(
    string Email,
    string Name,
    string PasswordHash = "");
