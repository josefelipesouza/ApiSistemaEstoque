using MediatR;
using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Item.BuscarPorCodigo;

public class BuscarPorCodigoItemHandler : IRequestHandler<BuscarPorCodigoItemRequest, ErrorOr<BuscarPorCodigoItemResponse>>
{
    private readonly IItemRepository _repository;

    public BuscarPorCodigoItemHandler(IItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<BuscarPorCodigoItemResponse>> Handle(
        BuscarPorCodigoItemRequest request,
        CancellationToken cancellationToken)
    {
        var item = await _repository.BuscarPorCodigoAsync(request.Codigo, cancellationToken);

        if (item is null)
        {
            return Errors.Application.ItemErrors.ItemNaoEncontrado;
        }

        var response = new BuscarPorCodigoItemResponse(
            item.Codigo,
            item.Descricao,
            item.QuantidadeMinima,
            item.Referencia,
            item.CodigoCategoria,
            item.CodigoUnidade,
            item.UsuarioCadastro,
            item.CreatedAt,
            item.updated_at,
            item.Inativo
        );

        return response;
    }
}
