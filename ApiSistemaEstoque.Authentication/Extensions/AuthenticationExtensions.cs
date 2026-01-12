using System.Text;
using ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Context;
using ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Models;
using ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Models.exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Extensions;

public static class AuthenticationExtensions
{
    // ======================================================
    // IDENTITY (API PURA - SEM COOKIE)
    // ======================================================
    public static IServiceCollection AddAuthenticationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // 🔹 DbContext do Identity
        services.AddDbContext<AuthContext>(options =>
            options.UseSqlite(
                configuration.GetConnectionString("EstoqueDbConnection")!
            )
        );

        // 🔹 IdentityCore (sem cookies, sem MVC)
        services.AddIdentityCore<IdentityUser>(options =>
        {
            options.User.RequireUniqueEmail = true;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<AuthContext>()
        .AddDefaultTokenProviders()
        .AddErrorDescriber<IdentityMensagensPortuguesConfig>();

        return services;
    }

    // ======================================================
    // JWT (ÚNICO SCHEME DE AUTH)
    // ======================================================
    public static IServiceCollection AddJwtServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection("Jwt");

        if (!jwtSection.Exists())
            throw new JwtException("Seção 'Jwt' não encontrada no appsettings");

        services.Configure<AppSettings>(jwtSection);

        var appSettings = jwtSection.Get<AppSettings>();

        if (appSettings is null ||
            string.IsNullOrWhiteSpace(appSettings.Secret) ||
            string.IsNullOrWhiteSpace(appSettings.Issuer) ||
            string.IsNullOrWhiteSpace(appSettings.Audience))
        {
            throw new JwtException("Configurações JWT inválidas ou incompletas");
        }

        var key = Encoding.UTF8.GetBytes(appSettings.Secret);

        // 🔹 ÚNICO AddAuthentication DA APLICAÇÃO
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ValidateIssuer = true,
                    ValidIssuer = appSettings.Issuer,

                    ValidateAudience = true,
                    ValidAudience = appSettings.Audience,

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        return services;
    }

    // ======================================================
    // HANDLERS (SE NECESSÁRIO NO FUTURO)
    // ======================================================
    public static IServiceCollection AddAuthenticationHandlers(
        this IServiceCollection services)
    {
        return services;
    }
}
