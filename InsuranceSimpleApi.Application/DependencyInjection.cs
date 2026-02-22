using FluentValidation;
using FluentValidation.AspNetCore;
using InsuranceSimpleApi.Application.Interfaces;
using InsuranceSimpleApi.Application.Models;
using InsuranceSimpleApi.Application.Services;
using InsuranceSimpleApi.Application.Validators;
using InsuranceSimpleApi.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace InsuranceSimpleApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
       
        services.AddScoped<PasswordService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<InsuranceReportService>();

        services.AddScoped<AuthenticatedUser>();
        services.AddScoped<IAuthenticatedUser>(sp =>
            sp.GetRequiredService<AuthenticatedUser>());


        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();

        services.AddInfrastructure(configuration);
        return services;
    }
}