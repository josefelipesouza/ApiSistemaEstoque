using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Auth;
using ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Auth;
using ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Context;
using ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Extensions
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<EstoqueContext>(options =>
                options.UseSqlite(
                    configuration.GetConnectionString("EstoqueDbConnection")
                ));

            services
                .AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<EstoqueContext>()
                .AddDefaultTokenProviders();

            // Repositórios
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IEstoqueRepository, EstoqueRepository>();
            services.AddScoped<IItemEstoqueRepository, ItemEstoqueRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<ITipoMovimentacaoRepository, TipoMovimentacaoRepository>();
            services.AddScoped<IMovimentacaoRepository, MovimentacaoRepository>();
            services.AddScoped<IUnidadeRepository, UnidadeRepository>();
            services.AddScoped<ITransporteRepository, TransporteRepository>();

            // Auth
            services.AddScoped<IUsuarioAuthService, UsuarioAuthService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();


            return services;
        }
    }
}
