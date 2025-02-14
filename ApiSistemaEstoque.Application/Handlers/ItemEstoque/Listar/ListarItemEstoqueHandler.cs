using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ErrorOr;
using MediatR;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.ItemEstoque.Listar;

public class ListarItemEstoqueHandler : BaseHandler, IRequestHandler<ListarItemEstoqueRequest, ErrorOr<IEnumerable<ListarItemEstoqueResponse>>>
{
    private readonly IItemEstoqueRepository _itemEstoqueRepository;

    public ListarItemEstoqueHandler(
        IMediator mediator,
        IItemEstoqueRepository itemEstoqueRepository) : base(mediator)
    {
        _itemEstoqueRepository = itemEstoqueRepository;
    }

    public async Task<ErrorOr<IEnumerable<ListarItemEstoqueResponse>>> Handle(ListarItemEstoqueRequest request, CancellationToken cancellationToken)
    {
        var itensEstoque = await _itemEstoqueRepository.ListarAsync(cancellationToken);

        var response = itensEstoque.Select(itemEstoque => new ListarItemEstoqueResponse(
            itemEstoque.Codigo,
            itemEstoque.CodigoItem,
            itemEstoque.CodigoEstoque,
            itemEstoque.Quantidade,
            itemEstoque.CreatedAt,
            itemEstoque.updated_at
        ));

        return response.ToList();
    }
}
