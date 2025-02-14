using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Context;
using ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Extensions
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configuração do DbContext para o sistema de estoque
            //services.AddDbContext<EstoqueContext>(options =>
                //options.UseSqlite(configuration.GetConnectionString("EstoqueDbConnection")!));

            // Configuração do DbContext para o sistema de estoque
            services.AddDbContext<EstoqueContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("EstoqueDbConnection")));

            // Registro dos repositórios
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IEstoqueRepository, EstoqueRepository>();
            services.AddScoped<IItemEstoqueRepository, ItemEstoqueRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IMovimentacaoRepository, MovimentacaoRepository>();
            services.AddScoped<IUnidadeRepository, UnidadeRepository>();
            

            return services;
        }
    }
}