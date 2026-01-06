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

var builder = WebApplication.CreateBuilder(args);

// ======================================================
// Apenas HTTP para simplificar
// ======================================================
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5295); // HTTP para Swagger
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
// SWAGGER
// ======================================================
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
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// ======================================================
// AUTENTICAÇÃO E USUÁRIO LOGADO
// ======================================================
builder.Services.AddAuthenticationServices(builder.Configuration);
builder.Services.AddJwtServices(builder.Configuration);
builder.Services.AddAuthenticationHandlers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUsuarioLogado, UsuarioLogado>();

// ======================================================
// INFRA & APP
// ======================================================
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

// ======================================================
// MEDIATR
// ======================================================
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(typeof(RegistrarUsuarioHandler).Assembly);
});

// ======================================================
// CONTROLLERS
// ======================================================
builder.Services.AddControllers()
    .AddApplicationPart(typeof(CategoriaController).Assembly);

var app = builder.Build();

// ======================================================
// SWAGGER PIPELINE
// ======================================================
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Sistema de Estoque v1");
    c.RoutePrefix = string.Empty; // Swagger na raiz
});

// ======================================================
// AUTORIZAÇÃO
// ======================================================
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// ======================================================
// Roda apenas Swagger
// ======================================================
app.Run();
