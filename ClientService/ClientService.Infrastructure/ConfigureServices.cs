using System.Text;
using ClientService.Application.Common.Interfaces;
using ClientService.Infrastructure.Persistence;
using ClientService.Infrastructure.Repositories;
using ClientService.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ClientService.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Testing
        services.AddHttpContextAccessor();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
        });

        // Initialize database and seeding
        services.AddScoped<ApplicationDbInitializer>();
        using var scope = services.BuildServiceProvider().CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbInitializer>();

        initializer.InitializeAsync().Wait();
        initializer.SeedAsync().Wait();

        // Jwt service
        services.AddScoped<IJwtService, JwtService>();

        // Google authentication
        services.AddScoped<IGoogleAuthService, GoogleAuthService>();


        // Authentication, Authorization
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // Validate JWT Token
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? throw new ArgumentException("Jwt:Key is required")))
                };
            });

        return services;
    }
}
