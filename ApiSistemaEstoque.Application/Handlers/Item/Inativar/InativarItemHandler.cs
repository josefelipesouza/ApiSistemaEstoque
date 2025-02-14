using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using MediatR;
using static ApiSistemaEstoque.ApiSistemaEstoque.Application.Errors.Application;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Item.Inativar;

public class InativarItemHandler
    : BaseHandler, IRequestHandler<InativarItemRequest, ErrorOr<bool>>
{
    private readonly IItemRepository _itemRepository;

    public InativarItemHandler(
        IItemRepository itemRepository,
        IMediator mediator) : base(mediator)
    {
        _itemRepository = itemRepository;
    }

    public async Task<ErrorOr<bool>> Handle(
        InativarItemRequest request,
        CancellationToken cancellationToken)
    {
        var validationErrors = Validar(request, new InativarItemRequestValidator());
        if (validationErrors.Any())
        {
            return validationErrors;
        }

        var item = await _itemRepository.BuscarPorCodigoAsync(request.Codigo, cancellationToken);

        if (item is null)
            return ItemErrors.ItemNaoEncontrado;

        item.SetInativar();
        _itemRepository.Atualizar(item);

        await _itemRepository.UnitOfWork.CommitAsync(cancellationToken);

        return true;
    }
}
