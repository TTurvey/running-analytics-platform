using FluentAssertions;
using Xunit;
using RunningAnalytics.Api.Tests.TestUtilities;
using RunningAnalytics.Infrastructure.Repositories;
using RunningAnalytics.Domain.Models;

namespace RunningAnalytics.Api.Tests.Users.Repository;

/// <summary>
/// Repository layer tests - focus on database persistence and data access.
/// Tests CRUD operations without validation concerns.
/// Note: ApplicationDbContext.OnModelCreating seeds 3 test users by default.
/// These tests work with or around that seeded data.
/// </summary>
public class UsersRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsSeededUsers_ByDefault()
    {
        var db = TestDbContextFactory.CreateInMemory();
        var repo = new UsersRepository(db);

        var users = await repo.GetAllAsync();

        // Default seed has 3 users
        users.Should().HaveCount(3);
        users.Should().Contain(u => u.Email == "testuser1@example.com");
        users.Should().Contain(u => u.Email == "testuser2@example.com");
        users.Should().Contain(u => u.Email == "testuser3@example.com");
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllUsers_WhenMultipleAdded()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var db = TestDbContextFactory.CreateAndSeed(
            new User { Id = id1, Email = "user1@example.com", Name = "User One", PasswordHash = "hash1" },
            new User { Id = id2, Email = "user2@example.com", Name = "User Two", PasswordHash = "hash2" }
        );

        var repo = new UsersRepository(db);
        var users = await repo.GetAllAsync();

        // Seeded 3 + added 2 = 5
        users.Should().HaveCount(5);
        users.Should().Contain(u => u.Id == id1 && u.Email == "user1@example.com");
        users.Should().Contain(u => u.Id == id2 && u.Email == "user2@example.com");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsUser_WhenUserExists()
    {
        var id = Guid.NewGuid();
        var db = TestDbContextFactory.CreateAndSeed(
            new User { Id = id, Email = "test@example.com", Name = "TestUser", PasswordHash = "hash" }
        );

        var repo = new UsersRepository(db);
        var user = await repo.GetByIdAsync(id);

        user.Should().NotBeNull();
        user!.Id.Should().Be(id);
        user.Email.Should().Be("test@example.com");
        user.Name.Should().Be("TestUser");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenUserDoesNotExist()
    {
        var db = TestDbContextFactory.CreateInMemory();
        var repo = new UsersRepository(db);

        var user = await repo.GetByIdAsync(Guid.NewGuid());

        user.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsSeededUser()
    {
        var db = TestDbContextFactory.CreateInMemory();
        var repo = new UsersRepository(db);

        // Use known seeded user ID
        var user = await repo.GetByIdAsync(Guid.Parse("8b7d3a1c-0e5f-4c63-9e91-1a2b3c4d5e6f"));

        user.Should().NotBeNull();
        user!.Email.Should().Be("testuser1@example.com");
    }

    [Fact]
    public async Task AddAsync_PersistsUser_AndReturnsCreatedUser()
    {
        var db = TestDbContextFactory.CreateInMemory();
        var repo = new UsersRepository(db);

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Email = "newuser@example.com",
            Name = "NewUser",
            PasswordHash = "hash123",
            CreatedAt = DateTime.UtcNow
        };

        var result = await repo.AddAsync(newUser);

        result.Id.Should().Be(newUser.Id);
        result.Email.Should().Be("newuser@example.com");

        // Verify persistence
        var retrieved = await repo.GetByIdAsync(newUser.Id);
        retrieved.Should().NotBeNull();
        retrieved!.Email.Should().Be("newuser@example.com");
    }

    [Fact]
    public async Task UpdateAsync_UpdatesUserData_WhenUserExists()
    {
        var id = Guid.Parse("8b7d3a1c-0e5f-4c63-9e91-1a2b3c4d5e6f"); // Use seeded user
        var db = TestDbContextFactory.CreateInMemory();

        var repo = new UsersRepository(db);
        var userToUpdate = new User { Id = id, Email = "updated@example.com", Name = "Updated", PasswordHash = "hash" };

        var result = await repo.UpdateAsync(userToUpdate);

        result.Should().BeTrue();

        // Verify persistence
        var retrieved = await repo.GetByIdAsync(id);
        retrieved.Should().NotBeNull();
        retrieved!.Email.Should().Be("updated@example.com");
        retrieved.Name.Should().Be("Updated");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsFalse_WhenUserDoesNotExist()
    {
        var db = TestDbContextFactory.CreateInMemory();
        var repo = new UsersRepository(db);

        var userToUpdate = new User
        {
            Id = Guid.NewGuid(),
            Email = "ghost@example.com",
            Name = "Ghost",
            PasswordHash = "hash"
        };

        var result = await repo.UpdateAsync(userToUpdate);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_RemovesUser_WhenUserExists()
    {
        var id = Guid.Parse("8b7d3a1c-0e5f-4c63-9e91-1a2b3c4d5e6f"); // Use seeded user
        var db = TestDbContextFactory.CreateInMemory();

        var repo = new UsersRepository(db);
        var result = await repo.DeleteAsync(id);

        result.Should().BeTrue();

        // Verify actually deleted
        var retrieved = await repo.GetByIdAsync(id);
        retrieved.Should().BeNull();

        // Verify others still exist
        var allUsers = await repo.GetAllAsync();
        allUsers.Should().HaveCount(2); // Started with 3, deleted 1
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenUserDoesNotExist()
    {
        var db = TestDbContextFactory.CreateInMemory();
        var repo = new UsersRepository(db);

        var result = await repo.DeleteAsync(Guid.NewGuid());

        result.Should().BeFalse();
    }
}
