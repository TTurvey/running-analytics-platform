using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using Xunit;
using RunningAnalytics.Api;
using RunningAnalytics.Application.DTOs;
using System.Net.Http.Json;
using RunningAnalytics.Api.Tests.TestUtilities;

namespace RunningAnalytics.Api.Tests.Users.Api;

public class DeleteUserTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public DeleteUserTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task DeleteUser_ReturnsNoContent_WhenSuccessful()
    {
        var createUserRequest = new CreateUserRequest{
            Email = "testuser@example.com", 
            PasswordHash = "password123", 
            Name = "TestUser"
        };

        var createdResponse = await _client.PostAsJsonAsync("/users", createUserRequest);
        createdResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await createdResponse.Content.ReadFromJsonAsync<UserResponse>();

        var response = await _client.DeleteAsync($"/users/{created!.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteUser_RemovesUserFromDatabase()
    {
        var createUserRequest = new CreateUserRequest
        {
            Email = "deleteme@example.com",
            PasswordHash = "password123",
            Name = "DeleteMe"
        };

        var createdResponse = await _client.PostAsJsonAsync("/users", createUserRequest);
        var created = await createdResponse.Content.ReadFromJsonAsync<UserResponse>();

        await _client.DeleteAsync($"/users/{created!.Id}");

        // Try to get the deleted user
        var getResponse = await _client.GetAsync($"/users/{created.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteUser_ReturnsNotFound_WhenUserDoesNotExist()
    {
        var response = await _client.DeleteAsync($"/users/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
