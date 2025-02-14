using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.Listar;

public record ListarMovimentacaoResponse(
    int? Codigo,
    StatusMovimentacao? Status,
    int? CodigoTipoMovimentacao,
    int? CodigoEstoqueSolicitante,
    int? CodigoUsuarioEstoqueSolicitante,
    int? CodigoEstoqueSolicitado,
    int? CodigoUsuarioEstoqueSolicitado,
    DateTime CreatedAt,
    DateTime updated_at,
    IEnumerable<ListarMovimentacaoItemResponse> ItensMovimentacao
);

public record ListarMovimentacaoItemResponse(
    int? Codigo,
    int? CodigoMovimentacao,
    int? Item,
    int? Quantidade
);
