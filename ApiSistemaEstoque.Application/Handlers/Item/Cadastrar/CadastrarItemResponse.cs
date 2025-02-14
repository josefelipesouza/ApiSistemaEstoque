using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Item.Cadastrar;

public record CadastrarItemResponse(
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
