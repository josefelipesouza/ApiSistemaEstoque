using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.Cadastrar;

public record CadastrarMovimentacaoResponse(
    int Codigo,
    StatusMovimentacao Status,
    int CodigoTipoMovimentacao,
    int CodigoEstoqueSolicitante,
    string CodigoUsuarioEstoqueSolicitante,
    int CodigoEstoqueSolicitado,
    string CodigoUsuarioEstoqueSolicitado,
    DateTime CreatedAt
);
