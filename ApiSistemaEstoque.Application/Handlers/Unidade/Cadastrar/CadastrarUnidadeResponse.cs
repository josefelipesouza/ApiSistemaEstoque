using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Unidade.Cadastrar;

public record CadastrarUnidadeResponse(
    int Codigo,
    string Descricao,
    int UsuarioCadastro,
    DateTime CreatedAt,
    DateTime updated_at,
    IEnumerable<Status> Inativo
);
