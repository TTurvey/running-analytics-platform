using Microsoft.EntityFrameworkCore;
using RunningAnalytics.Api.Models;

namespace RunningAnalytics.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.Parse("8b7d3a1c-0e5f-4c63-9e91-1a2b3c4d5e6f"),
                Email = "testuser1@example.com",
                PasswordHash = "TEST_ONLY_HASH_1",
                Name = "TestUser 1",
                CreatedAt = new DateTime(2026, 8, 1, 8, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = Guid.Parse("9c8e4b2d-1f60-5d74-af02-2b3c4d5e6f70"),
                Email = "testuser2@example.com",
                PasswordHash = "TEST_ONLY_HASH_2",
                Name = "TestUser 2",
                CreatedAt = new DateTime(2026, 8, 2, 9, 30, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = Guid.Parse("ad9f5c3e-2071-6e85-b013-3c4d5e6f7081"),
                Email = "testuser3@example.com",
                PasswordHash = "TEST_ONLY_HASH_3",
                Name = "TestUser 3",
                CreatedAt = new DateTime(2026, 8, 3, 10, 15, 0, DateTimeKind.Utc)
            });
    }
}