using FluentAssertions;
using Moq;
using Xunit;
using RunningAnalytics.Application.Services;
using RunningAnalytics.Application.Interfaces;
using RunningAnalytics.Application.DTOs;
using RunningAnalytics.Domain.Models;

namespace RunningAnalytics.Api.Tests.Users.Service;

/// <summary>
/// Service layer tests - focus on business logic and orchestration.
/// Tests DTO mapping and edge cases without touching validation or persistence.
/// Validation is handled by FluentValidation validators (tested in Validators/ folder).
/// Database persistence is tested in Repository/ folder tests.
/// </summary>
public class UsersServiceTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsEmptyList_WhenNoUsersExist()
    {
        var repo = new Mock<IUsersRepository>();
        repo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User>());

        var service = new UsersService(repo.Object);
        var result = await service.GetAllAsync();

        result.Should().BeEmpty();
        repo.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllUsers_AndMapsToDtos()
    {
        var users = new List<User>
        {
            new() { Id = Guid.NewGuid(), Email = "user1@example.com", Name = "User One", PasswordHash = "hash1" },
            new() { Id = Guid.NewGuid(), Email = "user2@example.com", Name = "User Two", PasswordHash = "hash2" }
        };
        var repo = new Mock<IUsersRepository>();
        repo.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

        var service = new UsersService(repo.Object);
        var result = await service.GetAllAsync();

        result.Should().HaveCount(2);
        result[0].Email.Should().Be("user1@example.com");
        result[0].Name.Should().Be("User One");
        result[1].Email.Should().Be("user2@example.com");
        result[1].Name.Should().Be("User Two");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsUser_WhenUserExists()
    {
        var id = Guid.NewGuid();
        var user = new User { Id = id, Email = "test@example.com", Name = "Test", PasswordHash = "hash" };
        var repo = new Mock<IUsersRepository>();
        repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(user);

        var service = new UsersService(repo.Object);
        var result = await service.GetByIdAsync(id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.Email.Should().Be("test@example.com");
        result.Name.Should().Be("Test");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenUserDoesNotExist()
    {
        var repo = new Mock<IUsersRepository>();
        repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);

        var service = new UsersService(repo.Object);
        var result = await service.GetByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenIdIsNull()
    {
        var repo = new Mock<IUsersRepository>();
        var service = new UsersService(repo.Object);

        var result = await service.GetByIdAsync(null);

        result.Should().BeNull();
        repo.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task AddAsync_CreatesUser_WithGeneratedIdAndCurrentTime()
    {
        var repo = new Mock<IUsersRepository>();
        repo.Setup(r => r.AddAsync(It.IsAny<User>()))
            .ReturnsAsync((User u) => u);

        var service = new UsersService(repo.Object);
        var request = new CreateUserRequest
        {
            Email = "newuser@example.com",
            PasswordHash = "password123",
            Name = "NewUser"
        };

        var result = await service.AddAsync(request);

        result.Id.Should().NotBeEmpty();
        result.Email.Should().Be("newuser@example.com");
        result.Name.Should().Be("NewUser");
        repo.Verify(r => r.AddAsync(It.Is<User>(u =>
            u.Email == "newuser@example.com" &&
            u.Name == "NewUser" &&
            u.PasswordHash == "password123"
        )), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesUser_AndReturnsTrueWhenSuccessful()
    {
        var id = Guid.NewGuid();
        var existingUser = new User { Id = id, Email = "old@example.com", Name = "Old", PasswordHash = "hash" };
        var repo = new Mock<IUsersRepository>();
        repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existingUser);
        repo.Setup(r => r.UpdateAsync(It.IsAny<User>())).ReturnsAsync(true);

        var service = new UsersService(repo.Object);
        var request = new UpdateUserRequest
        {
            Email = "new@example.com",
            PasswordHash = "newhash",
            Name = "New"
        };

        var result = await service.UpdateAsync(id, request);

        result.Should().BeTrue();
        repo.Verify(r => r.UpdateAsync(It.Is<User>(u =>
            u.Id == id &&
            u.Email == "new@example.com" &&
            u.Name == "New"
        )), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsFalse_WhenUserDoesNotExist()
    {
        var repo = new Mock<IUsersRepository>();
        repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);

        var service = new UsersService(repo.Object);
        var request = new UpdateUserRequest
        {
            Email = "ghost@example.com",
            PasswordHash = "hash",
            Name = "Ghost"
        };

        var result = await service.UpdateAsync(Guid.NewGuid(), request);

        result.Should().BeFalse();
        repo.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_CallsRepositoryDelete()
    {
        var id = Guid.NewGuid();
        var repo = new Mock<IUsersRepository>();
        repo.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

        var service = new UsersService(repo.Object);
        var result = await service.DeleteAsync(id);

        result.Should().BeTrue();
        repo.Verify(r => r.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenUserDoesNotExist()
    {
        var repo = new Mock<IUsersRepository>();
        repo.Setup(r => r.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        var service = new UsersService(repo.Object);
        var result = await service.DeleteAsync(Guid.NewGuid());

        result.Should().BeFalse();
    }
}
