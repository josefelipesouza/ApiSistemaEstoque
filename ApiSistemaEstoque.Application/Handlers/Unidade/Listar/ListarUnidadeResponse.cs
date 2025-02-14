using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Unidade.Listar;

public record ListarUnidadeResponse(
    int Codigo,
    string Descricao,
    int UsuarioCadastro,
    DateTime CreatedAt,
    DateTime updated_at,
    IEnumerable<Status> Inativo
);
