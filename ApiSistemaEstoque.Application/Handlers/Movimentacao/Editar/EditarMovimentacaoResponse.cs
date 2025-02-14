using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.Editar;

public record EditarMovimentacaoResponse(
    int Codigo,
    int CodigoUsuarioEstoqueSolicitado,
    StatusMovimentacao Status,
    DateTime UpdatedAt
);
