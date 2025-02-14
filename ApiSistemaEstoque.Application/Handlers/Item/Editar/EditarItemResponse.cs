using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Item.Editar;

public record EditarItemResponse(
    int Codigo,
    string Descricao,
    int QuantidadeMinima,
    string Referencia,
    int CodigoCategoria,
    int CodigoUnidade,
    int UsuarioCadastro,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    IEnumerable<Status> Inativo
);
