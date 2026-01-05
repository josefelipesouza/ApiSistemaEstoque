using Microsoft.OpenApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

// 🔹 Context
using ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Context;

// 🔹 Extensions
using ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Extensions;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Extensions;
using ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Extensions;

// Controllers / Handlers
using ApiSistemaEstoque.ApiSistemaEstoque.API.Controllers;
using ApiSistemaEstoque.Application.Handlers.Usuario.Auth.Registrar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Auth;

namespace ApiSistemaEstoque.ApiSistemaEstoque.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ======================================================
        // 1) KESTREL / HTTPS
        // ======================================================
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenLocalhost(5270, listenOptions => listenOptions.UseHttps());
        });

        // ======================================================
        // 2) SQLITE — STRING ÚNICA
        // ======================================================
        var databaseFolder = Path.Combine(AppContext.BaseDirectory, "Banco");
        Directory.CreateDirectory(databaseFolder);

        var databasePath = Path.Combine(databaseFolder, "estoque.db");

        builder.Configuration["ConnectionStrings:EstoqueDbConnection"] =
            $"Data Source={databasePath}";

        // ======================================================
        // 3) SWAGGER
        // ======================================================
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "API Sistema de Estoque",
                Version = "v1"
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Use: Bearer {token}",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        // ======================================================
        // 4) AUTH (USANDO SEUS MÉTODOS ✅)
        // ======================================================
        builder.Services.AddAuthenticationServices(builder.Configuration);
        builder.Services.AddJwtServices(builder.Configuration);
        builder.Services.AddAuthenticationHandlers();

        // ======================================================
        // 5) HTTP CONTEXT / USUÁRIO LOGADO
        // ======================================================
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IUsuarioLogado, UsuarioLogado>();

        // ======================================================
        // 6) INFRASTRUCTURE & APPLICATION
        // ======================================================
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddApplicationServices();

        // ======================================================
        // 7) MEDIATR
        // ======================================================
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(
                typeof(RegistrarUsuarioHandler).Assembly
            );
        });

        // ======================================================
        // 8) CONTROLLERS
        // ======================================================
        builder.Services.AddControllers()
            .AddApplicationPart(typeof(CategoriaController).Assembly);

        var app = builder.Build();

        // ======================================================
        // 9) PIPELINE
        // ======================================================
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

        app.MapControllers();

        app.Run();
    }
}
