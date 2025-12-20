using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.Editar;

public record EditarMovimentacaoResponse(
    int Codigo,
    string CodigoUsuarioEstoqueSolicitado,
    StatusMovimentacao Status,
    DateTime UpdatedAt
);
