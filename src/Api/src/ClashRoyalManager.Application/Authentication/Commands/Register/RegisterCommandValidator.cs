using FluentValidation;

namespace ClashRoyalManager.Application.Authentication.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(user => user.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Email invalid.");

        RuleFor(user => user.Password)
            .MinimumLength(8)
            .WithMessage("The password should be at least 8 characters.");
    }
}