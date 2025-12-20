using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.BuscarPorCodigo;

public record BuscarPorCodigoMovimentacaoResponse(
    int? Codigo,
    StatusMovimentacao? Status,
    int? CodigoTipoMovimentacao,
    int? CodigoEstoqueSolicitante,
    string? CodigoUsuarioEstoqueSolicitante,
    int? CodigoEstoqueSolicitado,
    string? CodigoUsuarioEstoqueSolicitado,
    DateTime CreatedAt,
    DateTime updated_at,
    IEnumerable<ItemMovimentacaoResponse> Itens);

public record ItemMovimentacaoResponse(
    int? Codigo,
    int? CodigoMovimentacao,
    int? Item,
    int? Quantidade
    );
