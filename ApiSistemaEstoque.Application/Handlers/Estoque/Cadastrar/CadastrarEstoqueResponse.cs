using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Estoque.Cadastrar;

public record CadastrarEstoqueResponse(
    int Codigo,
    string Descricao,
    string Localizacao,
    string Responsavel,
    int Superior,
    DateTime CreatedAt,
    DateTime updated_at,
    Status Inativo
);
