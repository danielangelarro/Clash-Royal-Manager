using ClashRoyalManager.Application.Entities;

namespace ClashRoyalManager.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}