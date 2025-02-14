using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.Cadastrar;

public record CadastrarMovimentacaoResponse(
    int Codigo,
    StatusMovimentacao Status,
    int CodigoTipoMovimentacao,
    int CodigoEstoqueSolicitante,
    int CodigoUsuarioEstoqueSolicitante,
    int CodigoEstoqueSolicitado,
    int CodigoUsuarioEstoqueSolicitado,
    DateTime CreatedAt
);
