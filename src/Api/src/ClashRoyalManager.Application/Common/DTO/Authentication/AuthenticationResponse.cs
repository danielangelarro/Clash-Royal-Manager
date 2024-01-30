namespace ClashRoyalManager.Application.DTO.Authentication;

public record AuthenticationResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Rol,
    string Photo,
    string Token);