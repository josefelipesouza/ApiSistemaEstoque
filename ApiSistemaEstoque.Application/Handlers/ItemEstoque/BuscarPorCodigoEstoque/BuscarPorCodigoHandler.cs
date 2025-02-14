using MediatR;
using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.ItemEstoque.BuscarPorCodigoEstoque;

public class BuscarPorCodigoHandler : IRequestHandler<BuscarPorCodigoRequest, ErrorOr<BuscarPorCodigoResponse>>
{
    private readonly IItemEstoqueRepository _repository;

    public BuscarPorCodigoHandler(IItemEstoqueRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<BuscarPorCodigoResponse>> Handle(
        BuscarPorCodigoRequest request,
        CancellationToken cancellationToken)
    {
        var itensEstoque = await _repository.BuscarPorCodigoEstoque(request.CodigoEstoque, cancellationToken);

        if (itensEstoque is null || !itensEstoque.Any())
        {
            return Errors.Application.ItemEstoqueErrors.ItemEstoqueNaoEncontrado;
        }

        var response = new BuscarPorCodigoResponse(
            itensEstoque.Select(item => new ItemEstoqueResponse(
                item.Codigo,
                item.CodigoItem,
                item.CodigoEstoque,
                item.Quantidade,
                item.CreatedAt,
                item.updated_at
            )).ToList()
        );

        return response;
    }
}
