using FluentValidation;
using Identity.Application.Contracts.Services;
using Identity.Application.Models.Parameters;
using Identity.Application.Services;
using Identity.Application.Validators;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shared.Implementations;

namespace Identity.Application.Configurations;

public static class DependencyInjectionApplication
{
    public static IServiceCollection GetValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<RegisterParameters>, RegisterParametersValidator>();
        services.AddScoped<IValidator<UserNewRoleParameters>, UserNewRoleParametersValidator>();
        services.AddScoped<IValidator<ForgotPasswordParameters>, ForgotPasswordParametersValidator>();
        services.AddScoped<IValidator<ResetPasswordParameters>, ResetTokenParametersValidator>();
        services.AddScoped<IValidator<LoginParameters>, LoginParametersValidator>();
        return services;
    }

    public static IServiceCollection GetDependencyInjectionApplication(this IServiceCollection services)
    {
        services.AddScoped<IIdentityEmailService, IdentityEmailService>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IUserService, UserService>();

        //Mail + Logger
        services.GetAccessoriesDependencyInjection();

        return services;
    }
}