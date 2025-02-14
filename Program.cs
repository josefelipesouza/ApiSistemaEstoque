using ApiSistemaEstoque.ApiSistemaEstoque.API.Controllers;
using ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Context;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.BuscarPorCodigo;
using Microsoft.OpenApi.Models;
namespace ApiSistemaEstoque.ApiSistemaEstoque.API.controllers;

/// <summary>
/// Classe principal da aplicação
/// </summary>
public partial class Program
{
    /// <summary>
    /// Ponto de entrada principal da aplicação
    /// </summary>
    /// <param name="args">Argumentos da linha de comando</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configuração explícita de URLs
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenLocalhost(5270, listenOptions =>
            {
                listenOptions.UseHttps(); // Configura HTTPS na porta 5270
            });
        });

        // Adiciona os serviços necessários ao container
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Sistema de Estoque", Version = "v1" });
            var xmlFile = $"{typeof(Program).Assembly.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }
        });

        builder.Services.AddScoped<EstoqueContext>();
        builder.Services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(typeof(BuscarPorCodigoCategoriaHandler).Assembly));
        builder.Services.AddControllers()
            .AddApplicationPart(typeof(CategoriaController).Assembly); // Certifique-se de que isso está correto

        var app = builder.Build();

        // Configuração do Swagger no ambiente de desenvolvimento
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                c.RoutePrefix = string.Empty; // Swagger diretamente na raiz
            });
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapGet("/", () => "Bem-vindo à API do Sistema de Estoque!");
        app.MapControllers(); // Mapeia os controllers
    }
}