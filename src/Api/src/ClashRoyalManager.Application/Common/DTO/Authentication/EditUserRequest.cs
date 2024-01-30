using Microsoft.AspNetCore.Http;

namespace ClashRoyalManager.Application.DTO.Authentication;

public record EditUserRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    IFormFile File
);