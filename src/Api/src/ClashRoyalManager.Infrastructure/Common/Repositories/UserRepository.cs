using ClashRoyalManager.Application.Common.Interfaces.Repository;
using ClashRoyalManager.Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClashRoyalManager.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    public readonly ClashRoyalManagerDBContext _context;

    public UserRepository(ClashRoyalManagerDBContext context)
    {
        _context = context;
    }

    public async Task Add(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
    
    public async Task EditUser(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetUserById(Guid id)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.ID_User == id);
    }
    
    public async Task<User?> GetUserByEmail(string email)
    {
        return await _context.Users.SingleOrDefaultAsync(user => user.Correo == email);
    }
}