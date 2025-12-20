using ApiSistemaEstoque.ApiSistemaEstoque.API.Controllers;
using ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Context;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using MediatR;
using ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Extensions;

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
        var jwtKey      = builder.Configuration["Jwt:Key"]     ?? throw new InvalidOperationException("Jwt:Key não configurado.");
        var jwtIssuer   = builder.Configuration["Jwt:Issuer"]  ?? throw new InvalidOperationException("Jwt:Issuer não configurado.");
        var jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience não configurado.");

        // 3) Registrar Swagger (antes de autenticação)
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Sistema de Estoque", Version = "v1" });

            // Suporte JWT no Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Use: Bearer {token}",
                Name        = "Authorization",
                In          = ParameterLocation.Header,
                Type        = SecuritySchemeType.Http,
                Scheme      = "Bearer",
                BearerFormat= "JWT"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                {
                    new OpenApiSecurityScheme{
                        Reference = new OpenApiReference{
                            Type = ReferenceType.SecurityScheme,
                            Id   = "Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });

        // 4) Serviços de infraestrutura, Mediator e Controllers
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddMediatR(typeof(Program).Assembly);
        builder.Services.AddControllers()
               .AddApplicationPart(typeof(CategoriaController).Assembly);

        // 5) Registrar o JWT Bearer **antes** do Identity, para definir
        //    o esquema padrão de autenticação como JWT
        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer           = true,
                    ValidateAudience         = true,
                    ValidateLifetime         = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer    = jwtIssuer,
                    ValidAudience  = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),

                    NameClaimType = ClaimTypes.NameIdentifier,
                    RoleClaimType = ClaimTypes.Role
                };
            });

        // 6) Agora registro o Identity (que registra cookies mas não
        //    sobrescreve o DefaultAuthenticateScheme já configurado acima)
        builder.Services
            .AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<EstoqueContext>()
            .AddDefaultTokenProviders();

        // 7) Build e pipeline
        var app = builder.Build();

        // 8) Swagger UI em Development
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Sistema de Estoque v1");
                c.RoutePrefix = string.Empty;
            });
        }

        // 9) Middlewares de segurança
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        // 10) Endpoints
        app.MapGet("/", () => "Bem-vindo à API do Sistema de Estoque!");
        app.MapControllers();

        app.Run();
    }
}
