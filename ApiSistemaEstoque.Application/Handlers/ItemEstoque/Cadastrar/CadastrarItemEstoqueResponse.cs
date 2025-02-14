namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.ItemEstoque.Cadastrar;

public record CadastrarItemEstoqueResponse(
    int Codigo,
    int CodigoItem,
    int CodigoEstoque,
    int Quantidade,
    DateTime CreatedAt,
    DateTime updated_at
);
