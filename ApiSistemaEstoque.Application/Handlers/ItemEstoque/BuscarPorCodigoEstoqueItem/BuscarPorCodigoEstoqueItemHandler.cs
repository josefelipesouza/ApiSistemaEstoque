using MediatR;
using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.ItemEstoque.BuscarPorCodigoEstoqueItem;

public class BuscarPorCodigoEstoqueItemHandler : IRequestHandler<BuscarPorCodigoEstoqueItemRequest, ErrorOr<BuscarPorCodigoEstoqueItemResponse>>
{
    private readonly IItemEstoqueRepository _repository;

    public BuscarPorCodigoEstoqueItemHandler(IItemEstoqueRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<BuscarPorCodigoEstoqueItemResponse>> Handle(
        BuscarPorCodigoEstoqueItemRequest request,
        CancellationToken cancellationToken)
    {
        var itemEstoque = await _repository.BuscarPorCodigoEstoqueItem(request.codigoEstoque , request.codigoItem, cancellationToken);

        if (itemEstoque is null)
        {
            return Errors.Application.ItemEstoqueErrors.ItemEstoqueNaoEncontrado;
        }

        var response = new BuscarPorCodigoEstoqueItemResponse(
            itemEstoque.Codigo,
            itemEstoque.CodigoItem,
            itemEstoque.CodigoEstoque,
            itemEstoque.Quantidade,
            itemEstoque.CreatedAt,
            itemEstoque.updated_at
        );

        return response;
    }
}
