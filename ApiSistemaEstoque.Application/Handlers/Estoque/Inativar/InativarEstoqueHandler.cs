using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using MediatR;
using static ApiSistemaEstoque.ApiSistemaEstoque.Application.Errors.Application;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Estoque.Inativar;

public class InativarEstoqueHandler
    : BaseHandler, IRequestHandler<InativarEstoqueRequest, ErrorOr<bool>>
{
    private readonly IEstoqueRepository _estoqueRepository;

    public InativarEstoqueHandler(
        IEstoqueRepository estoqueRepository,
        IMediator mediator) : base(mediator)
    {
        _estoqueRepository = estoqueRepository;
    }

    public async Task<ErrorOr<bool>> Handle(
    InativarEstoqueRequest request,
    CancellationToken cancellationToken)
    {
        var validationErrors = Validar(request, new InativarEstoqueRequestValidator());
        if (validationErrors.Any())
        {
            return validationErrors;
        }

        var estoque = await _estoqueRepository.BuscarPorCodigoAsync(request.Codigo, cancellationToken);

        if (estoque is null)
            return EstoqueErrors.EstoqueNaoEncontrado;

        _estoqueRepository.Inativar(estoque);

        await _estoqueRepository.UnitOfWork.CommitAsync(cancellationToken);

        return true;
    }
}
