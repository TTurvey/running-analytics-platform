using RunningAnalytics.Api.Models;

namespace RunningAnalytics.Api.Services;

public interface IUsersService
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid? id);
    Task<User> AddAsync(User user);
    //Task<bool> UpdateAsync(int id, User user);
    //Task<bool> DeleteAsync(int id);
}