using RunningAnalytics.Application.DTOs;

namespace RunningAnalytics.Application.Services;

public interface IUsersService
{
    Task<List<UserResponse>> GetAllAsync();
    Task<UserResponse?> GetByIdAsync(Guid? id);
    Task<UserResponse> AddAsync(CreateUserRequest obj);
    Task<bool> UpdateAsync(Guid id, UpdateUserRequest request);
    Task<bool> DeleteAsync(Guid id);
}