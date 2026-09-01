using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using Xunit;
using RunningAnalytics.Api;
using RunningAnalytics.Application.DTOs;
using RunningAnalytics.Api.Tests.TestUtilities;

namespace RunningAnalytics.Api.Tests.Users.Api;

public class CreateUserTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CreateUserTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateUser_ReturnsCreated_WithValidData()
    {
        var createUserRequest = new CreateUserRequest
        {
            Email = "testuser@example.com",
            PasswordHash = "password123",
            Name = "TestUser"
        };

        var response = await _client.PostAsJsonAsync("/users", createUserRequest);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await response.Content.ReadFromJsonAsync<UserResponse>();
        created.Should().NotBeNull();
        created!.Email.Should().Be("testuser@example.com");
        created.Name.Should().Be("TestUser");
    }

    [Fact]
    public async Task CreateUser_ReturnsCreatedAtAction_WithCorrectLocation()
    {
        var createUserRequest = new CreateUserRequest
        {
            Email = "anotheruser@example.com",
            PasswordHash = "password456",
            Name = "AnotherUser"
        };

        var response = await _client.PostAsJsonAsync("/users", createUserRequest);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location!.OriginalString.Should().Contain("/Users/");
    }

    [Fact]
    public async Task CreateUser_ReturnsBadRequest_WhenEmailIsInvalid()
    {
        var createUserRequest = new CreateUserRequest
        {
            Email = "not-an-email",
            PasswordHash = "password123",
            Name = "TestUser"
        };

        var response = await _client.PostAsJsonAsync("/users", createUserRequest);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateUser_ReturnsBadRequest_WhenNameIsEmpty()
    {
        var createUserRequest = new CreateUserRequest
        {
            Email = "valid@example.com",
            PasswordHash = "password123",
            Name = string.Empty
        };

        var response = await _client.PostAsJsonAsync("/users", createUserRequest);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
