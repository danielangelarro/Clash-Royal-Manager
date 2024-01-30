using ErrorOr;
using MediatR;
using ClashRoyalManager.Application.DTO.Authentication;
using Microsoft.AspNetCore.Http;

namespace ClashRoyalManager.Application.Authentication.Commands.EditUser;

public record EditUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    IFormFile File
) : IRequest<ErrorOr<AuthenticationResult>>;