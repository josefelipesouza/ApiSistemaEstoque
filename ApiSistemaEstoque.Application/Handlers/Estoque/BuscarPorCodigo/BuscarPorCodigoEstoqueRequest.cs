using MediatR;
using ErrorOr;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Estoque.BuscarPorCodigo;

public record BuscarPorCodigoEstoqueRequest : IRequest<ErrorOr<BuscarPorCodigoEstoqueResponse>>
{
    public int Codigo { get; set; }
}
