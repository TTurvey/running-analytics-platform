using FluentAssertions;
using FluentValidation;
using Xunit;
using RunningAnalytics.Application.DTOs;
using RunningAnalytics.Application.Validators;

namespace RunningAnalytics.Api.Tests.Users.Validators;

public class CreateUserValidatorTests
{
    private readonly CreateUserValidator _validator = new();

    [Fact]
    public void Validate_WithValidData_ShouldSucceed()
    {
        var request = new CreateUserRequest
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
        var request = new CreateUserRequest
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
        var request = new CreateUserRequest
        {
            Email = "not-an-email",
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
        var request = new CreateUserRequest
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
        var request = new CreateUserRequest
        {
            Email = "test@example.com",
            PasswordHash = "",
            Name = "Test User"
        };

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "PasswordHash");
    }

    [Fact]
    public void Validate_WithAllFieldsEmpty_ShouldFailWithMultipleErrors()
    {
        var request = new CreateUserRequest
        {
            Email = "",
            PasswordHash = "",
            Name = ""
        };

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        // Email has 2 errors (required + email format), Name has 1, PasswordHash has 1 = 4 total
        result.Errors.Should().HaveCountGreaterThanOrEqualTo(3);
    }
}
