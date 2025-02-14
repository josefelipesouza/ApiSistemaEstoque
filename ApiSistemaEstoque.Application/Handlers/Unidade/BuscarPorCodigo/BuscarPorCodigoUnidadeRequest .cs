using MediatR;
using ErrorOr;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Unidade.BuscarPorCodigo;

public record BuscarPorCodigoUnidadeRequest : IRequest<ErrorOr<BuscarPorCodigoUnidadeResponse>>
{
    public int Codigo { get; set; }
}
