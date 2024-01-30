using FluentValidation;

namespace ClashRoyalManager.Application.Authentication.Queries.Login;

public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
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