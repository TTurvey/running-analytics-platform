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

}