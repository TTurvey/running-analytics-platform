using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using Xunit;
using RunningAnalytics.Api;
using RunningAnalytics.Application.DTOs;
using RunningAnalytics.Api.Tests.TestUtilities;

namespace RunningAnalytics.Api.Tests.Users.Api;

public class GetUsersTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public GetUsersTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllUsers_ReturnsOk_WithEmptyList()
    {
        var response = await _client.GetAsync("/users");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var users = await response.Content.ReadFromJsonAsync<List<UserResponse>>();
        users.Should().NotBeNull();
        users.Should().BeOfType<List<UserResponse>>();
    }

    [Fact]
    public async Task GetAllUsers_ReturnsOk_WithMultipleUsers()
    {
        // Create multiple users
        var user1 = new CreateUserRequest
        {
            Email = "user1@example.com",
            PasswordHash = "pass123",
            Name = "User One"
        };
        var user2 = new CreateUserRequest
        {
            Email = "user2@example.com",
            PasswordHash = "pass456",
            Name = "User Two"
        };

        await _client.PostAsJsonAsync("/users", user1);
        await _client.PostAsJsonAsync("/users", user2);

        var response = await _client.GetAsync("/users");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var users = await response.Content.ReadFromJsonAsync<List<UserResponse>>();
        users.Should().NotBeNull();
        users.Should().HaveCountGreaterThanOrEqualTo(2);
        users!.Should().Contain(u => u.Email == "user1@example.com");
        users.Should().Contain(u => u.Email == "user2@example.com");
    }
}
