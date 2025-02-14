using MediatR;
using ErrorOr;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.ItemEstoque.BuscarPorCodigoEstoqueItem;

public record BuscarPorCodigoEstoqueItemRequest : IRequest<ErrorOr<BuscarPorCodigoEstoqueItemResponse>>
{
    public int codigoEstoque {get; set; }
    public int codigoItem {get; set; }
}


