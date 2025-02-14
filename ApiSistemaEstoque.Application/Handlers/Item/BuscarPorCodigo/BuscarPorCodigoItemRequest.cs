using MediatR;
using ErrorOr;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Item.BuscarPorCodigo;

public record BuscarPorCodigoItemRequest : IRequest<ErrorOr<BuscarPorCodigoItemResponse>>
{
    public int Codigo { get; set; }
}
