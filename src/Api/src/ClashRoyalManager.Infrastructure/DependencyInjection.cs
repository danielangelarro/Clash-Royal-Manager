using ClashRoyalManager.Application.Common.Interfaces;
using ClashRoyalManager.Application.Common.Interfaces.Authentication;
using ClashRoyalManager.Application.Common.Interfaces.Repository;
using ClashRoyalManager.Application.Common.Interfaces.Services;
using ClashRoyalManager.Application.Entities;
using ClashRoyalManager.Application.Services;
using ClashRoyalManager.Infrastructure.Authentication;
using ClashRoyalManager.Infrastructure.Repositories;
using ClashRoyalManager.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClashRoyalManager.Infrastructure;


public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ClashRoyalManagerDBContext>
{
    public ClashRoyalManagerDBContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(@Directory.GetCurrentDirectory() + "/../ClashRoyalManager.API/appsettings.Development.json")
            .Build();
        var builder = new DbContextOptionsBuilder<ClashRoyalManagerDBContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        builder.UseNpgsql(connectionString);
        return new ClashRoyalManagerDBContext(builder.Options);
    }
}


public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        Microsoft.Extensions.Configuration.ConfigurationManager configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        services.AddDbContext<ClashRoyalManagerDBContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                x => x.MigrationsAssembly("ClashRoyalManager.API")
            )
        );

        services.AddSingleton<IJwtTokenGenerator, JwTokenGenerator>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IGetCurrentUserLoginService, GetCurrentUserLoginService>();

        services.AddHostedService<CheckedCaduceDateProductService>();
        
        return services;
    }
}