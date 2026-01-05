using MediatR;
using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Auth;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.Cadastrar;

public class CadastrarMovimentacaoHandler 
    : BaseHandler, IRequestHandler<CadastrarMovimentacaoRequest, ErrorOr<CadastrarMovimentacaoResponse>>
{
    private readonly IMovimentacaoRepository _movimentacaoRepository;
    private readonly IUsuarioLogado _usuarioLogado;

    public CadastrarMovimentacaoHandler(
        IMediator mediator,
        IMovimentacaoRepository movimentacaoRepository,
        IUsuarioLogado usuarioLogado
    ) : base(mediator)
    {
        _movimentacaoRepository = movimentacaoRepository;
        _usuarioLogado = usuarioLogado;
    }

    public async Task<ErrorOr<CadastrarMovimentacaoResponse>> Handle(
        CadastrarMovimentacaoRequest request,
        CancellationToken cancellationToken)
    {
        if (Validar(request, new CadastrarMovimentacaoRequestValidator()) is var resultado 
            && resultado.Count != 0)
            return resultado;

        var usuarioCadastro = _usuarioLogado.ObterUsuarioId();

        if (string.IsNullOrWhiteSpace(usuarioCadastro))
            return Errors.Application.UsuarioErrors.UsuarioNaoAutenticado;

        // Criação da movimentação
        var movimentacao = new Domain.Entities.Movimentacao(
            request.CodigoTipoMovimentacao,
            request.CodigoEstoqueSolicitante,
            usuarioCadastro,
            request.CodigoEstoqueSolicitado ?? 0
        );

        await _movimentacaoRepository.AdicionarAsync(movimentacao, cancellationToken);
        await _movimentacaoRepository.UnitOfWork.CommitAsync(cancellationToken);

        foreach (var item in request.Itens)
        {
            var itemMovimentacao = new Domain.Entities.ItemMovimentacao(
                movimentacao.Codigo,
                item.CodigoItem,
                item.Quantidade
            );

            await _movimentacaoRepository.AdicionarItemAsync(
                itemMovimentacao, cancellationToken);
        }

        await _movimentacaoRepository.UnitOfWork.CommitAsync(cancellationToken);

        return new CadastrarMovimentacaoResponse(
            movimentacao.Codigo,
            movimentacao.Status,
            movimentacao.CodigoTipoMovimentacao,
            movimentacao.CodigoEstoqueSolicitante,
            movimentacao.CodigoUsuarioEstoqueSolicitante,
            movimentacao.CodigoEstoqueSolicitado,
            movimentacao.CodigoUsuarioEstoqueSolicitado,
            movimentacao.CreatedAt
        );
    }
}
