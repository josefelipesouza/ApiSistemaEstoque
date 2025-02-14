using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Item.BuscarPorCodigo;

public record BuscarPorCodigoItemResponse(
    int Codigo,
    string Descricao,
    int QuantidadeMinima,
    string Referencia,
    int CodigoCategoria,
    int CodigoUnidade,
    int UsuarioCadastro,
    DateTime CreatedAt,
    DateTime updated_at,
    IEnumerable<Status> Inativo
);
