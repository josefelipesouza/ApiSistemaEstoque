using ErrorOr;
using MediatR;
using Microsoft.Extensions.DependencyInjection; // Para IServiceCollection
using Microsoft.AspNetCore.Http;                // Para IHttpContextAccessor


// Handlers para Categoria
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.BuscarPorCodigo;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.Cadastrar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.Editar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.Inativar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.Listar;

// Handlers para Estoque
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Estoque.BuscarPorCodigo;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Estoque.Cadastrar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Estoque.Editar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Estoque.Inativar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Estoque.Listar;

// Handlers para Item
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Item.BuscarPorCodigo;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Item.Cadastrar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Item.Editar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Item.Inativar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Item.Listar;

// Handlers para Movimentação
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.Buscar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.BuscarPorCodigo;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.Cadastrar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.Editar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.Listar;

// Handlers para Unidade
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Unidade.BuscarPorCodigo;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Unidade.Cadastrar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Unidade.Editar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Unidade.Inativar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Unidade.Listar;

// Handlers para ItemEstoque
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.ItemEstoque.BuscarPorCodigoEstoqueItem;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.ItemEstoque.BuscarPorCodigoEstoque;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.ItemEstoque.Cadastrar;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.ItemEstoque.Listar;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Extensions;

/// <summary>
/// Extensões para configuração dos serviços da camada de aplicação
/// </summary>
public static class ApplicationExtensions
{
    /// <summary>
    /// Adiciona os serviços da camada de aplicação ao container DI
    /// </summary>
    /// <param name="services">A coleção de serviços</param>
    /// <returns>A coleção de serviços atualizada</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Handlers para Categoria
        services.AddScoped<IRequestHandler<BuscarPorCodigoCategoriaRequest, ErrorOr<BuscarPorCodigoCategoriaResponse>>, BuscarPorCodigoCategoriaHandler>();
        services.AddScoped<IRequestHandler<CadastrarCategoriaRequest, ErrorOr<CadastrarCategoriaResponse>>, CadastrarCategoriaHandler>();
        services.AddScoped<IRequestHandler<EditarCategoriaRequest, ErrorOr<EditarCategoriaResponse>>, EditarCategoriaHandler>();
        services.AddScoped<IRequestHandler<InativarCategoriaRequest, ErrorOr<bool>>, InativarCategoriaHandler>();
        services.AddScoped<IRequestHandler<ListarCategoriaRequest, ErrorOr<IEnumerable<ListarCategoriaResponse>>>, ListarCategoriaHandler>();

        // Handlers para Estoque
        services.AddScoped<IRequestHandler<BuscarPorCodigoEstoqueRequest, ErrorOr<BuscarPorCodigoEstoqueResponse>>, BuscarPorCodigoEstoqueHandler>();
        services.AddScoped<IRequestHandler<CadastrarEstoqueRequest, ErrorOr<CadastrarEstoqueResponse>>, CadastrarEstoqueHandler>();
        services.AddScoped<IRequestHandler<EditarEstoqueRequest, ErrorOr<EditarEstoqueResponse>>, EditarEstoqueHandler>();
        services.AddScoped<IRequestHandler<InativarEstoqueRequest, ErrorOr<bool>>, InativarEstoqueHandler>();
        services.AddScoped<IRequestHandler<ListarEstoqueRequest, ErrorOr<IEnumerable<ListarEstoqueResponse>>>, ListarEstoqueHandler>();

        // Handlers para Item
        services.AddScoped<IRequestHandler<BuscarPorCodigoItemRequest, ErrorOr<BuscarPorCodigoItemResponse>>, BuscarPorCodigoItemHandler>();
        services.AddScoped<IRequestHandler<CadastrarItemRequest, ErrorOr<CadastrarItemResponse>>, CadastrarItemHandler>();
        services.AddScoped<IRequestHandler<EditarItemRequest, ErrorOr<EditarItemResponse>>, EditarItemHandler>();
        services.AddScoped<IRequestHandler<InativarItemRequest, ErrorOr<bool>>, InativarItemHandler>();
        services.AddScoped<IRequestHandler<ListarItemRequest, ErrorOr<IEnumerable<ListarItemResponse>>>, ListarItemHandler>();

        // Handlers para Item de Estoque
        services.AddScoped<IRequestHandler<BuscarPorCodigoRequest, ErrorOr<BuscarPorCodigoResponse>>, BuscarPorCodigoHandler>();
        services.AddScoped<IRequestHandler<BuscarPorCodigoEstoqueItemRequest, ErrorOr<BuscarPorCodigoEstoqueItemResponse>>, BuscarPorCodigoEstoqueItemHandler>();
        services.AddScoped<IRequestHandler<CadastrarItemEstoqueRequest, ErrorOr<CadastrarItemEstoqueResponse>>, CadastrarItemEstoqueHandler>();
        services.AddScoped<IRequestHandler<ListarItemEstoqueRequest, ErrorOr<IEnumerable<ListarItemEstoqueResponse>>>, ListarItemEstoqueHandler>();

        // Handlers para Movimentação
        services.AddScoped<IRequestHandler<BuscarMovimentacaoRequest, ErrorOr<IEnumerable<BuscarMovimentacaoResponse>>>, BuscarMovimentacaoHandler>();
        services.AddScoped<IRequestHandler<BuscarPorCodigoMovimentacaoRequest, ErrorOr<BuscarPorCodigoMovimentacaoResponse>>, BuscarPorCodigoMovimentacaoHandler>();
        services.AddScoped<IRequestHandler<CadastrarMovimentacaoRequest, ErrorOr<CadastrarMovimentacaoResponse>>, CadastrarMovimentacaoHandler>();
        services.AddScoped<IRequestHandler<EditarMovimentacaoRequest, ErrorOr<EditarMovimentacaoResponse>>, EditarMovimentacaoHandler>();
        services.AddScoped<IRequestHandler<ListarMovimentacaoRequest, ErrorOr<IEnumerable<ListarMovimentacaoResponse>>>, ListarMovimentacaoHandler>();

        // Handlers para Unidade
        services.AddScoped<IRequestHandler<BuscarPorCodigoUnidadeRequest, ErrorOr<BuscarPorCodigoUnidadeResponse>>, BuscarPorCodigoUnidadeHandler>();
        services.AddScoped<IRequestHandler<CadastrarUnidadeRequest, ErrorOr<CadastrarUnidadeResponse>>, CadastrarUnidadeHandler>();
        services.AddScoped<IRequestHandler<EditarUnidadeRequest, ErrorOr<EditarUnidadeResponse>>, EditarUnidadeHandler>();
        services.AddScoped<IRequestHandler<InativarUnidadeRequest, ErrorOr<bool>>, InativarUnidadeHandler>();
        services.AddScoped<IRequestHandler<ListarUnidadeRequest, ErrorOr<IEnumerable<ListarUnidadeResponse>>>, ListarUnidadeHandler>();

        return services;
    }
}
