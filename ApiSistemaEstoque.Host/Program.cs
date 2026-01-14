using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using MediatR;

// 🔹 Extensions
using ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Extensions;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Extensions;
using ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Extensions;

// Controllers / Handlers
using ApiSistemaEstoque.ApiSistemaEstoque.API.Controllers;
using ApiSistemaEstoque.Application.Handlers.Usuario.Auth.Registrar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Auth;
using ApiSistemaEstoque.API.Auth;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ======================================================
        // KESTREL
        // ======================================================
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenLocalhost(5295);
        });

        // ======================================================
        // SQLITE
        // ======================================================
        var databaseFolder = Path.Combine(AppContext.BaseDirectory, "Banco");
        Directory.CreateDirectory(databaseFolder);

        var databasePath = Path.Combine(databaseFolder, "estoque.db");
        builder.Configuration["ConnectionStrings:EstoqueDbConnection"] =
            $"Data Source={databasePath}";

        // ======================================================
        // CONTROLLERS
        // ======================================================
        builder.Services
            .AddControllers()
            .AddApplicationPart(typeof(CategoriaController).Assembly);

        // ======================================================
        // AUTHENTICATION / IDENTITY / JWT
        // ======================================================
        builder.Services.AddAuthenticationServices(builder.Configuration);
        builder.Services.AddJwtServices(builder.Configuration);
        builder.Services.AddAuthenticationHandlers();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IUsuarioLogado, UsuarioLogado>();

        // ======================================================
        // 🔐 JWT COMO SCHEME PADRÃO
        // ======================================================
        builder.Services.PostConfigure<AuthenticationOptions>(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        });

        // ======================================================
        // 🔓 AUTHORIZATION (SEM FALLBACK POLICY)
        // ======================================================
        builder.Services.AddAuthorization();

        // ======================================================
        // SWAGGER
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
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Informe: Bearer {token}"
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
        // INFRA / APPLICATION
        // ======================================================
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddApplicationServices();

        // ======================================================
        // MEDIATR
        // ======================================================
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(
                typeof(RegistrarUsuarioHandler).Assembly
            );
        });

        var app = builder.Build();

        // ======================================================
        // PIPELINE
        // ======================================================
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Sistema de Estoque v1");
            c.RoutePrefix = string.Empty;
        });

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
