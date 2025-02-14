using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Unidade.Editar;

public record EditarUnidadeResponse(
    int Codigo,
    string Descricao,
    int UsuarioCadastro,
    DateTime CreatedAt,
    DateTime updated_at,
    IEnumerable<Status> Inativo
);
