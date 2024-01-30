using Microsoft.Extensions.DependencyInjection;
using ClashRoyalManager.Application.Authentication.Queries.Login;
using ClashRoyalManager.Application.Authentication.Commands.Register;
using ClashRoyalManager.Application.Authentication.Services;
using FluentValidation;
using ClashRoyalManager.Application.Authentication.Commands.EditUser;

namespace ClashRoyalManager.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.AddScoped<IValidator<RegisterCommand>, RegisterCommandValidator>();
        services.AddScoped<IValidator<LoginQuery>, LoginQueryValidator>();
        services.AddScoped<IValidator<EditUserCommand>, EditUserCommandValidator>();

        services.AddScoped<PasswordService>();

        return services;
    }
}