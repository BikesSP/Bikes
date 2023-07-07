using System.Reflection;
using ClientService.Application.Common.Behaviours;
using ClientService.Application.Common.Mappings;
using ClientService.Application.Services.CurrentUserService;
using ClientService.Application.Services.GoogleAuthService;
using ClientService.Application.Services.JwtService;
using ClientService.Application.Services.Storage;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClientService.Application;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Auto mapper
        services.AddAutoMapper(typeof(MappingProfiles));

        // FluentAPI validation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationErrorBehaviour<,>));
        });

        // Jwt service
        services.AddScoped<IJwtService, JwtService>();

        // Google authentication
        services.AddScoped<IGoogleAuthService, GoogleAuthService>();

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped<IStorageService, StorageService>();

        return services;
    }
}
