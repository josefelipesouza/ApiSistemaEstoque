using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.Buscar;

public record BuscarMovimentacaoResponse(
    int? Codigo,
    StatusMovimentacao? Status,
    int? CodigoTipoMovimentacao,
    int? CodigoEstoqueSolicitante,
    int? CodigoUsuarioEstoqueSolicitante,
    int? CodigoEstoqueSolicitado,
    int? CodigoUsuarioEstoqueSolicitado,
    DateTime CreatedAt,
    DateTime updated_at,
    IEnumerable<BuscarMovimentacaoItemResponse> ItensMovimentacao
);

public record BuscarMovimentacaoItemResponse(
    int? Codigo,
    int? CodigoMovimentacao,
    int? CodigoProduto,
    int? Quantidade
);
