using FluentAssertions;
using Xunit;
using RunningAnalytics.Application.DTOs;
using RunningAnalytics.Application.Validators;

namespace RunningAnalytics.Api.Tests.Users.Validators;

public class UpdateUserValidatorTests
{
    private readonly UpdateUserValidator _validator = new();

    [Fact]
    public void Validate_WithValidData_ShouldSucceed()
    {
        var request = new UpdateUserRequest
        {
            Email = "test@example.com",
            PasswordHash = "password123",
            Name = "Test User"
        };

        var result = _validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithEmptyEmail_ShouldFail()
    {
        var request = new UpdateUserRequest
        {
            Email = "",
            PasswordHash = "password123",
            Name = "Test User"
        };

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Validate_WithInvalidEmail_ShouldFail()
    {
        var request = new UpdateUserRequest
        {
            Email = "invalid-email",
            PasswordHash = "password123",
            Name = "Test User"
        };

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email" && e.ErrorMessage.Contains("valid email"));
    }

    [Fact]
    public void Validate_WithEmptyName_ShouldFail()
    {
        var request = new UpdateUserRequest
        {
            Email = "test@example.com",
            PasswordHash = "password123",
            Name = ""
        };

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Validate_WithEmptyPasswordHash_ShouldFail()
    {
        var request = new UpdateUserRequest
        {
            Email = "test@example.com",
            PasswordHash = "",
            Name = "Test User"
        };

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "PasswordHash");
    }
}
