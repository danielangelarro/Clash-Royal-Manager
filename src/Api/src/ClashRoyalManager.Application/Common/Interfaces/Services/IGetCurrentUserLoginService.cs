using ClashRoyalManager.Application.Entities;

namespace ClashRoyalManager.Application.Services;

public interface IGetCurrentUserLoginService
{
    Task<User> Handle(string token);
}