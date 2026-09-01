using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using Xunit;
using RunningAnalytics.Api;
using RunningAnalytics.Application.DTOs;
using RunningAnalytics.Api.Tests.TestUtilities;

namespace RunningAnalytics.Api.Tests.Users.Api;

public class GetUserByIdTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public GetUserByIdTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetUserById_ReturnsOk_WithValidId()
    {
        var createUserRequest = new CreateUserRequest
        {
            Email = "testuser@example.com",
            PasswordHash = "password123",
            Name = "Test User"
        };
        var createResponse = await _client.PostAsJsonAsync("/users", createUserRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<UserResponse>();

        var response = await _client.GetAsync($"/users/{created!.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var user = await response.Content.ReadFromJsonAsync<UserResponse>();
        user.Should().NotBeNull();
        user!.Id.Should().Be(created.Id);
        user.Email.Should().Be("testuser@example.com");
        user.Name.Should().Be("Test User");
    }

    [Fact]
    public async Task GetUserById_ReturnsNotFound_WhenUserDoesNotExist()
    {
        var response = await _client.GetAsync($"/users/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetUserById_ReturnsUserData_WithCorrectStructure()
    {
        var createUserRequest = new CreateUserRequest
        {
            Email = "fulldata@example.com",
            PasswordHash = "password123",
            Name = "Full Data User"
        };
        var createResponse = await _client.PostAsJsonAsync("/users", createUserRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<UserResponse>();

        var response = await _client.GetAsync($"/users/{created!.Id}");
        var user = await response.Content.ReadFromJsonAsync<UserResponse>();

        user.Should().NotBeNull();
        user!.Id.Should().NotBeEmpty();
        user.Email.Should().NotBeEmpty();
        user.Name.Should().NotBeEmpty();
    }
}
