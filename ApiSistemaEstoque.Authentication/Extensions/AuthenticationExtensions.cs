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
    // IDENTITY (API PURA - SEM COOKIE / SEM MVC)
    // ======================================================
    public static IServiceCollection AddAuthenticationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // 🔹 DbContext exclusivo do Identity
        services.AddDbContext<AuthContext>(options =>
            options.UseSqlite(
                configuration.GetConnectionString("EstoqueDbConnection")!
            )
        );

        // 🔹 IdentityCore → NÃO registra Cookie Authentication
        services.AddIdentityCore<IdentityUser>(options =>
        {
            options.User.RequireUniqueEmail = true;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<AuthContext>()
        .AddSignInManager() // necessário para login manual (PasswordSignInAsync)
        .AddDefaultTokenProviders()
        .AddErrorDescriber<IdentityMensagensPortuguesConfig>();

        return services;
    }

    // ======================================================
    // JWT (ÚNICO E VERDADEIRO SCHEME DE AUTENTICAÇÃO)
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

        // 🔹 JWT como esquema padrão (NUNCA Cookie)
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false; // true em produção
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
    // HANDLERS (RESERVADO PARA O FUTURO)
    // ======================================================
    public static IServiceCollection AddAuthenticationHandlers(
        this IServiceCollection services)
    {
        // ❗ NÃO registrar Cookie, PolicyScheme ou Redirect aqui
        return services;
    }
}
