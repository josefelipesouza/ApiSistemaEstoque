namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.ItemEstoque.BuscarPorCodigoEstoqueItem;

public record BuscarPorCodigoEstoqueItemResponse(
    int Codigo,
    int CodigoItem,
    int CodigoEstoque,
    int Quantidade,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
