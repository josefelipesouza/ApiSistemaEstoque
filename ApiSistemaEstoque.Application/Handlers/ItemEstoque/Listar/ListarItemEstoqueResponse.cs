namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.ItemEstoque.Listar;

public record ListarItemEstoqueResponse(
    int? Codigo,
    int? CodigoItem,
    int? CodigoEstoque,
    int? Quantidade,
    DateTime CreatedAt,
    DateTime updated_at
);
