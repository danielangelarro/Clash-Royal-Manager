using ErrorOr;
using ClashRoyalManager.Application.Authentication.Commands.EditUser;
using ClashRoyalManager.Application.Authentication.Commands.Register;
using ClashRoyalManager.Application.Authentication.Queries.Login;
using ClashRoyalManager.Application.Common.Interfaces.Repository;
using ClashRoyalManager.Application.DTO.Authentication;
using ClashRoyalManager.Application.Entities;
using ClashRoyalManager.Application.Entradas.Commands.CurrentUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClashRoyalManager.API.Controllers;

[Route("auth")]
public class AuthenticationController : ApiController
{
    private readonly ISender _mediator;

    public AuthenticationController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = new RegisterCommand(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password
        );

        ErrorOr<AuthenticationResult> authResult = await _mediator.Send(command);

        return authResult.Match(
            authResult => Ok(MapAuthResult(authResult)),
            errors => Problem(errors)
        );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = new LoginQuery(request.Email, request.Password);

        ErrorOr<AuthenticationResult> authResult = await _mediator.Send(query);

        return authResult.Match(
            authResult => Ok(MapAuthResult(authResult)),
            errors => Problem(errors)
        );
    }

    [HttpPost("edit-user")]
    public async Task<IActionResult> EditUser([FromForm] EditUserRequest request)
    {
        var command = new EditUserCommand(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password,
            request.File
        );

        ErrorOr<AuthenticationResult> authResult = await _mediator.Send(command);

        return authResult.Match(
            authResult => Ok(MapAuthResult(authResult)),
            errors => Problem(errors)
        );
    }

    [HttpGet("current-user/{token}")]
    public async Task<IActionResult> CurrentUser(string Token)
    {
        var command = new GetCurrentUserQuery(Token);

        ErrorOr<AuthenticationResult> authResult = await _mediator.Send(command);

        return authResult.Match(
            authResult => Ok(MapAuthResult(authResult)),
            errors => Problem(errors)
        );
    }

    private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
    {
        return new AuthenticationResponse(
            authResult.user.ID_User,
            authResult.user.Nombres,
            authResult.user.Apellidos,
            authResult.user.Correo,
            authResult.user.Rol,
            authResult.user.Photo,
            authResult.Token
        );
    }
}
