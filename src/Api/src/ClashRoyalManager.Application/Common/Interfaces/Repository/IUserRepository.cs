using ClashRoyalManager.Application.Entities;

namespace ClashRoyalManager.Application.Common.Interfaces.Repository;

public interface IUserRepository
{
    Task<User?> GetUserById(Guid id);
    Task<User?> GetUserByEmail(string email);
    Task Add(User user);
    Task EditUser(User user);
}