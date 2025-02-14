namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Estoque.Cadastrar;

public record CadastrarEstoqueResponse(
    int Codigo,
    string Descricao,
    string Localizacao,
    int Responsavel,
    int Superior,
    DateTime CreatedAt,
    DateTime updated_at,
    bool? Inativo
);
