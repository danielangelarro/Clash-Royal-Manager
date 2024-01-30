using ClashRoyalManager.Application.Entities;

namespace ClashRoyalManager.Application.DTO.Authentication;

public record AuthenticationResult(User user, string Token);