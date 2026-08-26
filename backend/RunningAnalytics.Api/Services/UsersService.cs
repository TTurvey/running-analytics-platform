using Microsoft.EntityFrameworkCore;
using RunningAnalytics.Api.Models;
using RunningAnalytics.Api.Data;

namespace RunningAnalytics.Api.Services;

public class UsersService(ApplicationDbContext context) : IUsersService
{
    public async Task<List<User>> GetAllAsync()
    {
        return await context
        .Users
        .OrderBy(u => u.CreatedAt)
        .ToListAsync();
    }

    public async Task<User?> GetByIdAsync(Guid? id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<User> AddAsync(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> UpdateAsync(Guid id, User user)
    {
        var existingUser = await context.Users.FindAsync(id);
        if (existingUser == null) return false;

        existingUser.Email = user.Email;
        existingUser.PasswordHash = user.PasswordHash;
        existingUser.Name = user.Name;

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