using ClashRoyalManager.Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClashRoyalManager.Infrastructure.Repositories;

public class ClashRoyalManagerDBContext : DbContext
{
    public ClashRoyalManagerDBContext(DbContextOptions<ClashRoyalManagerDBContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }

    public DbSet<User> Users {get; set; }
}