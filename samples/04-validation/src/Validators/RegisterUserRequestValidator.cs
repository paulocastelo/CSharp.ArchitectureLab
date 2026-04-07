using FluentValidation;
using Validation.Sample.Contracts;

namespace Validation.Sample.Validators;

public sealed class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    private static readonly HashSet<string> ExistingEmails =
    [
        "demo@sample.local",
        "admin@sample.local"
    ];

    public RegisterUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .Must(email => !ExistingEmails.Contains(email.Trim().ToLowerInvariant()))
            .WithMessage("Email is already registered.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(100);
    }
}
