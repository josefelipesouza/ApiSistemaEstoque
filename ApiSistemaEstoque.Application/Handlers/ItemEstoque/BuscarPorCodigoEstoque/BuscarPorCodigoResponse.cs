namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.ItemEstoque.BuscarPorCodigoEstoque;

public record BuscarPorCodigoResponse(
    IEnumerable<ItemEstoqueResponse> ItensEstoque
);

public record ItemEstoqueResponse(
    int Codigo,
    int CodigoItem,
    int CodigoEstoque,
    int Quantidade,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
