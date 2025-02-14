using ErrorOr;
using MediatR;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Unidade.Editar;

public class EditarUnidadeHandler : BaseHandler, IRequestHandler<EditarUnidadeRequest, ErrorOr<EditarUnidadeResponse>>
{
    private readonly IUnidadeRepository _unidadeRepository;

    public EditarUnidadeHandler(
        IMediator mediator,
        IUnidadeRepository unidadeRepository) : base(mediator)
    {
        _unidadeRepository = unidadeRepository;
    }

    public async Task<ErrorOr<EditarUnidadeResponse>> Handle(EditarUnidadeRequest request, CancellationToken cancellationToken)
    {
        var validator = new EditarUnidadeRequest.EditarUnidadeRequestValidator();
        var validationResult = validator.Validate(request);
        
        if (!validationResult.IsValid)
            return validationResult.Errors
                .Select(e => Error.Validation(e.PropertyName, e.ErrorMessage))
                .ToList();

        var unidadeExistente = await _unidadeRepository.BuscarPorCodigoAsync(request.Codigo, cancellationToken);

        if (unidadeExistente is null)
            return Errors.Application.UnidadeErrors.UnidadeNaoEncontrada;

        unidadeExistente.SetDescricao(request.Descricao);
        unidadeExistente.SetDataAlteracao(DateTime.UtcNow);

        _unidadeRepository.Atualizar(unidadeExistente);
        
        await _unidadeRepository.UnitOfWork.CommitAsync(cancellationToken);

        return new EditarUnidadeResponse(
            unidadeExistente.Codigo,
            unidadeExistente.Descricao,
            unidadeExistente.UsuarioCadastro,
            unidadeExistente.CreatedAt,
            unidadeExistente.updated_at,
            unidadeExistente.Inativo
        );
    }
}
