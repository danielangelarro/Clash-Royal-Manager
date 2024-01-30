using ClashRoyalManager.Domain.Common.Errors;
using ClashRoyalManager.Application.Common.Interfaces.Authentication;
using ClashRoyalManager.Application.Common.Interfaces.Repository;
using ClashRoyalManager.Application.Entities;
using ClashRoyalManager.Application.DTO.Authentication;
using ClashRoyalManager.Application.Authentication.Services;
using FluentValidation;
using ErrorOr;
using MediatR;

namespace ClashRoyalManager.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly PasswordService _passwordService;
    private readonly IValidator<RegisterCommand> _validator;

    public RegisterCommandHandler(
        IUserRepository userRepository, 
        IJwtTokenGenerator jwtTokenGenerator, 
        IValidator<RegisterCommand> validator,
        PasswordService passwordService)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _validator = validator;
        _passwordService = passwordService;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command);

        if (!validationResult.IsValid)
        {
            return Errors.Model.ModelsInvalid;
        }

        if (await _userRepository.GetUserByEmail(command.Email) is not null)
        {
            return Errors.User.DuplicateEmail;
        }

        var user = new User {
            Nombres = command.FirstName,
            Apellidos = command.LastName,
            Correo = command.Email,
            Password = _passwordService.HashPassword(command.Password)
        };

        _userRepository.Add(user);

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(
            user,
            token
        );
    }
}
