using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using MediatR;

// Verifique se esses Namespaces abaixo batem com as pastas do seu projeto
using ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Context;
using ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Extensions;
using ApiSistemaEstoque.ApiSistemaEstoque.API.Controllers;

namespace ApiSistemaEstoque.ApiSistemaEstoque.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // 1) Configurar Kestrel / HTTPS
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenLocalhost(5270, listenOptions => listenOptions.UseHttps());
        });

        // 2) Carregar configuração do JWT
        var jwtKey = builder.Configuration.GetValue<string>("Jwt:Key") 
                     ?? throw new InvalidOperationException("Jwt:Key não encontrado.");
        var jwtIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer") 
                        ?? throw new InvalidOperationException("Jwt:Issuer não encontrado.");
        var jwtAudience = builder.Configuration.GetValue<string>("Jwt:Audience") 
                          ?? throw new InvalidOperationException("Jwt:Audience não encontrado.");

        // 3) Registrar Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Sistema de Estoque", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Use: Bearer {token}",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });
        });

        // 4) Serviços
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddHttpContextAccessor();
        
        builder.Services.AddMediatR(typeof(Program).Assembly);
        
        builder.Services.AddControllers()
               .AddApplicationPart(typeof(CategoriaController).Assembly);

        // 5) Autenticação JWT
        builder.Services
            .AddAuthentication(options =>
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
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    NameClaimType = ClaimTypes.NameIdentifier,
                    RoleClaimType = ClaimTypes.Role
                };
            });

        // 6) Identity
        builder.Services
            .AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<EstoqueContext>()
            .AddDefaultTokenProviders();

        var app = builder.Build();

        // 8) Swagger UI
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
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapGet("/", () => "Bem-vindo à API do Sistema de Estoque!");
        app.MapControllers();

        app.Run();
    }
}