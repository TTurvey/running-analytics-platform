using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using Xunit;
using RunningAnalytics.Api;
using RunningAnalytics.Application.DTOs;
using RunningAnalytics.Api.Tests.TestUtilities;

namespace RunningAnalytics.Api.Tests.Users.Api;

public class UpdateUserTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public UpdateUserTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task UpdateUser_ReturnsNoContent_WithValidData()
    {
        var createUserRequest = new CreateUserRequest
        {
            Email = "testuser@example.com",
            PasswordHash = "password123",
            Name = "TestUser"
        };
        var createdResponse = await _client.PostAsJsonAsync("/users", createUserRequest);
        var created = await createdResponse.Content.ReadFromJsonAsync<UserResponse>();

        var updateUserRequest = new UpdateUserRequest
        {
            Email = "newemail@example.com",
            PasswordHash = "newpassword123",
            Name = "UpdatedUser"
        };

        var response = await _client.PutAsJsonAsync($"/users/{created!.Id}", updateUserRequest);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateUser_ReturnsNoContent_AndPersistsChanges()
    {
        var createUserRequest = new CreateUserRequest
        {
            Email = "original@example.com",
            PasswordHash = "password123",
            Name = "OriginalName"
        };
        var createdResponse = await _client.PostAsJsonAsync("/users", createUserRequest);
        var created = await createdResponse.Content.ReadFromJsonAsync<UserResponse>();

        var updateUserRequest = new UpdateUserRequest
        {
            Email = "updated@example.com",
            PasswordHash = "newpassword",
            Name = "UpdatedName"
        };

        await _client.PutAsJsonAsync($"/users/{created!.Id}", updateUserRequest);

        var getResponse = await _client.GetAsync($"/users/{created.Id}");
        var updated = await getResponse.Content.ReadFromJsonAsync<UserResponse>();

        updated!.Email.Should().Be("updated@example.com");
        updated.Name.Should().Be("UpdatedName");
    }

    [Fact]
    public async Task UpdateUser_ReturnsBadRequest_WhenEmailIsInvalid()
    {
        var createUserRequest = new CreateUserRequest
        {
            Email = "original@example.com",
            PasswordHash = "password123",
            Name = "OriginalName"
        };
        var createdResponse = await _client.PostAsJsonAsync("/users", createUserRequest);
        var created = await createdResponse.Content.ReadFromJsonAsync<UserResponse>();

        var updateUserRequest = new UpdateUserRequest
        {
            Email = "not-an-email",
            PasswordHash = "newpassword",
            Name = "UpdatedName"
        };

        var response = await _client.PutAsJsonAsync($"/users/{created!.Id}", updateUserRequest);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateUser_ReturnsBadRequest_WhenNameIsEmpty()
    {
        var createUserRequest = new CreateUserRequest
        {
            Email = "existing@example.com",
            PasswordHash = "password123",
            Name = "ExistingUser"
        };
        var createdResponse = await _client.PostAsJsonAsync("/users", createUserRequest);
        var created = await createdResponse.Content.ReadFromJsonAsync<UserResponse>();

        var updateUserRequest = new UpdateUserRequest
        {
            Email = "updated@example.com",
            PasswordHash = "newpassword",
            Name = string.Empty
        };

        var response = await _client.PutAsJsonAsync($"/users/{created!.Id}", updateUserRequest);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateUser_ReturnsNotFound_WhenUserDoesNotExist()
    {
        var updateUserRequest = new UpdateUserRequest{
            Email = "ghost@example.com", 
            PasswordHash = "password", 
            Name = "TestUserGhost"
        };

        var response = await _client.PutAsJsonAsync($"/users/{Guid.NewGuid()}", updateUserRequest);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
