using FluentValidation;
using RunningAnalytics.Application.DTOs;

namespace RunningAnalytics.Application.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be a valid email address");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");

        RuleFor(x => x.PasswordHash)
            .NotEmpty().WithMessage("PasswordHash is required");
    }
}
