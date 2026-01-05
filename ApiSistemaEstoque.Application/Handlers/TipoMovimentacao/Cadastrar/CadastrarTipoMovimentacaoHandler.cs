using ErrorOr;
using MediatR;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Auth;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.TipoMovimentacao.Cadastrar;

public class CadastrarTipoMovimentacaoHandler 
    : BaseHandler, IRequestHandler<CadastrarTipoMovimentacaoRequest, ErrorOr<CadastrarTipoMovimentacaoResponse>>
{
    private readonly ITipoMovimentacaoRepository _tipoMovimentacaoRepository;
    private readonly IUsuarioLogado _usuarioLogado;

    public CadastrarTipoMovimentacaoHandler(
        IMediator mediator,
        ITipoMovimentacaoRepository tipoMovimentacaoRepository,
        IUsuarioLogado usuarioLogado
    ) : base(mediator)
    {
        _tipoMovimentacaoRepository = tipoMovimentacaoRepository;
        _usuarioLogado = usuarioLogado;
    }

    public async Task<ErrorOr<CadastrarTipoMovimentacaoResponse>> Handle(
        CadastrarTipoMovimentacaoRequest request,
        CancellationToken cancellationToken)
    {
        if (Validar(request, new CadastrarTipoMovimentacaoRequestValidator()) is var resultado
            && resultado.Count != 0)
            return resultado;

        var usuarioCadastro = _usuarioLogado.ObterUsuarioId();

        if (string.IsNullOrWhiteSpace(usuarioCadastro))
            return Errors.Application.UsuarioErrors.UsuarioNaoAutenticado;

        var nova = new Domain.Entities.TipoMovimentacao(
            request.Tipo,
            request.Descricao,
            usuarioCadastro
        );

        await _tipoMovimentacaoRepository.AdicionarAsync(nova, cancellationToken);
        await _tipoMovimentacaoRepository.UnitOfWork.CommitAsync(cancellationToken);

        return new CadastrarTipoMovimentacaoResponse(
            nova.Codigo,
            nova.Tipo,
            nova.Descricao!,
            nova.UsuarioCadastro,
            nova.CreatedAt,
            nova.updated_at,
            nova.Inativo
        );
    }
}
