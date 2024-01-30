using ErrorOr;
using MediatR;
using ClashRoyalManager.Application.DTO.Authentication;

namespace ClashRoyalManager.Application.Authentication.Queries.Login;

public record LoginQuery(
    string Email,
    string Password
) : IRequest<ErrorOr<AuthenticationResult>>;