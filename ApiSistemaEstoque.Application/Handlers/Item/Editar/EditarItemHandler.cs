using ErrorOr;
using MediatR;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Item.Editar;

public class EditarItemHandler : BaseHandler, IRequestHandler<EditarItemRequest, ErrorOr<EditarItemResponse>>
{
    private readonly IItemRepository _itemRepository;

    public EditarItemHandler(
        IMediator mediator,
        IItemRepository itemRepository) : base(mediator)
    {
        _itemRepository = itemRepository;
    }

    public async Task<ErrorOr<EditarItemResponse>> Handle(EditarItemRequest request, CancellationToken cancellationToken)
    {
        var validator = new EditarItemRequest.EditarItemRequestValidator();
        var validationResult = validator.Validate(request);
        
        if (!validationResult.IsValid)
            return validationResult.Errors
                .Select(e => Error.Validation(e.PropertyName, e.ErrorMessage))
                .ToList();

        var itemExistente = await _itemRepository.BuscarPorCodigoAsync(request.Codigo, cancellationToken);

        if (itemExistente is null)
            return Errors.Application.ItemErrors.ItemNaoEncontrado;

        itemExistente.SetDescricao(request.Descricao);
        itemExistente.SetQuantidadeMinima(request.QuantidadeMinima);
        itemExistente.SetReferencia(request.Referencia);
        itemExistente.SetCodigoCategoria(request.CodigoCategoria);
        itemExistente.SetCodigoUnidade(request.CodigoUnidade);
        itemExistente.SetDataAlteracao(DateTime.UtcNow);

        _itemRepository.Atualizar(itemExistente);
        
        await _itemRepository.UnitOfWork.CommitAsync(cancellationToken);

        return new EditarItemResponse(
            itemExistente.Codigo,
            itemExistente.Descricao,
            itemExistente.QuantidadeMinima,
            itemExistente.Referencia,
            itemExistente.CodigoCategoria,
            itemExistente.CodigoUnidade,
            itemExistente.UsuarioCadastro,
            itemExistente.CreatedAt,
            itemExistente.updated_at,
            itemExistente.Inativo
        );
    }
}
