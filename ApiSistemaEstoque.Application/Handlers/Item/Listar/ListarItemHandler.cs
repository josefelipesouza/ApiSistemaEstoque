using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ErrorOr;
using MediatR;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Item.Listar;

public class ListarItemHandler : BaseHandler, IRequestHandler<ListarItemRequest, ErrorOr<IEnumerable<ListarItemResponse>>>
{
    private readonly IItemRepository _itemRepository;

    public ListarItemHandler(
        IMediator mediator,
        IItemRepository itemRepository) : base(mediator)
    {
        _itemRepository = itemRepository;
    }

    public async Task<ErrorOr<IEnumerable<ListarItemResponse>>> Handle(ListarItemRequest request, CancellationToken cancellationToken)
    {
        var itens = await _itemRepository.ListarAsync(cancellationToken);

        var response = itens.Select(item => new ListarItemResponse(
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
        ));

        return response.ToList();
    }
}
