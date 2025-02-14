using MediatR;
using ErrorOr;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.BuscarPorCodigo;

public record BuscarPorCodigoCategoriaRequest : IRequest<ErrorOr<BuscarPorCodigoCategoriaResponse>>
{
    public int Codigo { get; set; }
}
