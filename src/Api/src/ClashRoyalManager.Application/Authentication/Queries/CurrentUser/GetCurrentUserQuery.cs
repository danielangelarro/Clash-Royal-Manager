using MediatR;
using ErrorOr;
using ClashRoyalManager.Application.DTO.Authentication;

namespace ClashRoyalManager.Application.Entradas.Commands.CurrentUser;

public record GetCurrentUserQuery(
   string Token
) : IRequest<ErrorOr<AuthenticationResult>>;

