using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using MediatR;
using static ApiSistemaEstoque.ApiSistemaEstoque.Application.Errors.Application;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Unidade.Inativar;

public class InativarUnidadeHandler
    : BaseHandler, IRequestHandler<InativarUnidadeRequest, ErrorOr<bool>>
{
    private readonly IUnidadeRepository _unidadeRepository;

    public InativarUnidadeHandler(
        IUnidadeRepository unidadeRepository,
        IMediator mediator) : base(mediator)
    {
        _unidadeRepository = unidadeRepository;
    }

    public async Task<ErrorOr<bool>> Handle(
        InativarUnidadeRequest request,
        CancellationToken cancellationToken)
    {
        var validationErrors = Validar(request, new InativarUnidadeRequestValidator());
        if (validationErrors.Any())
        {
            return validationErrors;
        }

        var unidade = await _unidadeRepository.BuscarPorCodigoAsync(request.Codigo, cancellationToken);

        if (unidade is null)
            return UnidadeErrors.UnidadeNaoEncontrada;

        unidade.SetInativar();
        _unidadeRepository.Atualizar(unidade);

        await _unidadeRepository.UnitOfWork.CommitAsync(cancellationToken);

        return true;
    }
}
