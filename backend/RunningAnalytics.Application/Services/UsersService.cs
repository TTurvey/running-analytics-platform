using RunningAnalytics.Application.DTOs;
using RunningAnalytics.Application.Interfaces;
using RunningAnalytics.Domain.Models;

namespace RunningAnalytics.Application.Services;

public class UsersService(IUsersRepository repository) : IUsersService
{
    public async Task<List<UserResponse>> GetAllAsync()
    {
        var users = await repository.GetAllAsync();
        return users.Select(ToResponse).ToList();
    }

    public async Task<UserResponse?> GetByIdAsync(Guid? id)
    {
        if (id is null) return null;

        var user = await repository.GetByIdAsync(id.Value);
        return user is null ? null : ToResponse(user);
    }

    public async Task<UserResponse> AddAsync(CreateUserRequest request)
    {
        var user = new User
        {
            Name = request.Name,
            Email = request.Email
        };

        return ToResponse(await repository.AddAsync(user));
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateUserRequest request)
    {
        var user = await repository.GetByIdAsync(id);
        if (user is null) return false;

        user.Name = request.Name;
        user.Email = request.Email;

        return await repository.UpdateAsync(user);
    }

    public Task<bool> DeleteAsync(Guid id) =>
        repository.DeleteAsync(id);

    private static UserResponse ToResponse(User user) =>
        new()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
}