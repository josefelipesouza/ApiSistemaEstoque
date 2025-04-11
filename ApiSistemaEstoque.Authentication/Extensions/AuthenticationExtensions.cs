using System.Text;
using ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Context;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Services;
//using ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Models;
using ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Models.exceptions;
/*
using ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Handlers.AlterarSenha;
using ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Handlers.Login;
using ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Handlers.Registrar;
using ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Handlers.SolicitarEsqueciSenha;
*/
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Extensions;

    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AuthContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("EstoqueDbConnection")!));

            services.AddIdentity<IdentityUser , IdentityRole>(x => x.User.RequireUniqueEmail = true)
                .AddEntityFrameworkStores<AuthContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<IdentityMensagensPortuguesConfig>();

            return services;
        }

        public static IServiceCollection AddJwtServices(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection(nameof(AppSettings));
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();

            if (appSettings is null)
                throw new JwtException("Configurações token inválidas");

            var key = Encoding.ASCII.GetBytes(appSettings.Segredo);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                bearerOptions.RequireHttpsMetadata = true;
                bearerOptions.SaveToken = true;
                bearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = appSettings.ValidoEm,
                    ValidIssuer = appSettings.Emissor
                };
            });

            return services;
        }

        public static IServiceCollection AddAuthenticationHandlers(this IServiceCollection services)
        {
            /*
            services.AddScoped<IRequestHandler<LoginRequest, ErrorOr<LoginResponse>>, LoginHandler>();
            services.AddScoped<IRequestHandler<RegistrarRequest, ErrorOr<bool>>, RegistrarHandler>();
            services.AddScoped<IRequestHandler<SolicitarEsqueciSenhaRequest, ErrorOr<string?>>, SolicitarEsqueciSenhaHandler>();
            services.AddScoped<IRequestHandler<AlterarSenhaRequest, ErrorOr<string?>>, AlterarSenhaHandler>();
            */

            return services;
        }

        /*

        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            return services;
        }
        */
    }
