using ErrorOr;
using MediatR;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using static ApiSistemaEstoque.ApiSistemaEstoque.Application.Errors.Application;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.TipoMovimentacao.Inativar;

public class InativarTipoMovimentacaoHandler 
   : BaseHandler,  IRequestHandler<InativarTipoMovimentacaoRequest, ErrorOr<bool>>
{
    private readonly ITipoMovimentacaoRepository _tipoMovimentacaoRepository;

    public InativarTipoMovimentacaoHandler(ITipoMovimentacaoRepository tipoMovimentacaoRepository, IMediator mediator) : base(mediator)
    {
        _tipoMovimentacaoRepository = tipoMovimentacaoRepository;
    }

    public async Task<ErrorOr<bool>> Handle(InativarTipoMovimentacaoRequest request, CancellationToken cancellationToken)
    {

        var validationErrors = Validar(request, new InativarTipoMovimentacaoRequestValidator());
        if (validationErrors.Any())
        {
            return validationErrors;
        }

        var tipoMov = await _tipoMovimentacaoRepository.BuscarPorCodigoAsync(request.Codigo, cancellationToken);

        if (tipoMov is null)
            return TipoMovimentacaoErrors.TipoMovimentacaoNaoEncontrada;

        _tipoMovimentacaoRepository.Inativar(tipoMov);
        
        await _tipoMovimentacaoRepository.UnitOfWork.CommitAsync(cancellationToken);

        return true;
    }
}
