using ApiSistemaEstoque.ApiSistemaEstoque.API.Controllers;
using ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Context;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.BuscarPorCodigo;
using Microsoft.OpenApi.Models;
using ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using MediatR;

namespace ApiSistemaEstoque.ApiSistemaEstoque.API.controllers;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configurar HTTPS explícito (porta 5270)
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenLocalhost(5270, listenOptions =>
            {
                listenOptions.UseHttps();
            });
        });

        // Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Sistema de Estoque", Version = "v1" });

            // XML de documentação
            var xmlFile = $"{typeof(Program).Assembly.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }

            // Suporte a JWT no Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header usando o esquema Bearer. Ex: 'Bearer {seu token}'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });

        // Configurações obrigatórias do JWT (Issuer, Audience, Key)
        var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key não configurado.");
        var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer não configurado.");
        var jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience não configurado.");

        // Injeções de dependência
        builder.Services.AddScoped<EstoqueContext>();
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddMediatR(typeof(Program).Assembly);
        builder.Services.AddControllers()
            .AddApplicationPart(typeof(CategoriaController).Assembly);

        // Identity
        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<EstoqueContext>()
            .AddDefaultTokenProviders();

        // Autenticação JWT
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

                NameClaimType = ClaimTypes.NameIdentifier,
                RoleClaimType = ClaimTypes.Role
            };

        });

        var app = builder.Build();

        // Swagger
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Sistema de Estoque v1");
                c.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();

        // Middleware de autenticação/autorização
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapGet("/", () => "Bem-vindo à API do Sistema de Estoque!");
        app.MapControllers();

        app.Run();
    }
}
