using RunningAnalytics.Api.Models;
using RunningAnalytics.Api.Data;
using RunningAnalytics.Api.dtos;
using Microsoft.EntityFrameworkCore;

namespace RunningAnalytics.Api.Services;

public class UsersService(ApplicationDbContext context) : IUsersService
{
    public async Task<List<UserResponse>> GetAllAsync()
    {
        return await context
            .Users
            .Select(user => new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
            })
            .ToListAsync();
    }

    public async Task<UserResponse?> GetByIdAsync(Guid? id)
    {
        var result = await context
            .Users
            .Where(user => user.Id == id)
            .Select(user => new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
            })
            //.FindAsync(id)
            .FirstOrDefaultAsync();

        return result;
    }

    public async Task<UserResponse> AddAsync(CreateUserRequest request)
    {
        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = request.PasswordHash,
            Name = request.Name,
            CreatedAt = DateTime.UtcNow
        };

        context.Users.Add(newUser);
        await context.SaveChangesAsync();

        return new UserResponse
        {
            Id = newUser.Id,
            Email = request.Email,
            Name = request.Name,
            CreatedAt = newUser.CreatedAt,
        };
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateUserRequest request)
    {
        var existingUser = await context.Users.FindAsync(id);
        if (existingUser == null) return false;

        existingUser.Email = request.Email;
        existingUser.PasswordHash = request.PasswordHash;
        existingUser.Name = request.Name;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var userToDelete = await context.Users.FindAsync(id);
        if (userToDelete == null) return false;

        context.Users.Remove(userToDelete);
        await context.SaveChangesAsync();
        return true;
    }
}