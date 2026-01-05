using ErrorOr;
using MediatR;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Auth;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Unidade.Editar;

public class EditarUnidadeHandler 
    : BaseHandler, IRequestHandler<EditarUnidadeRequest, ErrorOr<EditarUnidadeResponse>>
{
    private readonly IUnidadeRepository _unidadeRepository;
    private readonly IUsuarioLogado _usuarioLogado;

    public EditarUnidadeHandler(
        IMediator mediator,
        IUnidadeRepository unidadeRepository,
        IUsuarioLogado usuarioLogado
    ) : base(mediator)
    {
        _unidadeRepository = unidadeRepository;
        _usuarioLogado = usuarioLogado;
    }

    public async Task<ErrorOr<EditarUnidadeResponse>> Handle(
        EditarUnidadeRequest request,
        CancellationToken cancellationToken)
    {
        var validator = new EditarUnidadeRequest.EditarUnidadeRequestValidator();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
            return validationResult.Errors
                .Select(e => Error.Validation(e.PropertyName, e.ErrorMessage))
                .ToList();

        var usuarioCadastro = _usuarioLogado.ObterUsuarioId();

        if (string.IsNullOrWhiteSpace(usuarioCadastro))
            return Errors.Application.UsuarioErrors.UsuarioNaoAutenticado;

        var unidadeExistente =
            await _unidadeRepository.BuscarPorCodigoAsync(request.Codigo, cancellationToken);

        if (unidadeExistente is null)
            return Errors.Application.UnidadeErrors.UnidadeNaoEncontrada;

        unidadeExistente.SetDescricao(request.Descricao);
        unidadeExistente.SetUsuarioCadastro(usuarioCadastro);
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
