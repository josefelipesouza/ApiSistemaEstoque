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
    public static IServiceCollection AddAuthenticationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AuthContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("EstoqueDbConnection")!));

        services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<AuthContext>()
        .AddDefaultTokenProviders()
        .AddErrorDescriber<IdentityMensagensPortuguesConfig>();

        return services;
    }

    public static IServiceCollection AddJwtServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // 🔹 SEÇÃO CORRETA
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

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
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

    public static IServiceCollection AddAuthenticationHandlers(
        this IServiceCollection services)
    {
        return services;
    }
}
