using RunningAnalytics.Domain.Models;

namespace RunningAnalytics.Application.Interfaces;

public interface IUsersRepository
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task<User> AddAsync(User user);
    Task<bool> UpdateAsync(User user);
    Task<bool> DeleteAsync(Guid id);
}