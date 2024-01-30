using ErrorOr;
using MediatR;
using ClashRoyalManager.Application.DTO.Authentication;

namespace ClashRoyalManager.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password
) : IRequest<ErrorOr<AuthenticationResult>>;