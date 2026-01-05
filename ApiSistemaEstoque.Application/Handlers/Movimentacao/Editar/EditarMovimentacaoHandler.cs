using ErrorOr;
using MediatR;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Auth;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.Editar;

public class EditarMovimentacaoHandler 
    : BaseHandler, IRequestHandler<EditarMovimentacaoRequest, ErrorOr<EditarMovimentacaoResponse>>
{
    private readonly IMovimentacaoRepository _movimentacaoRepository;
    private readonly IUsuarioLogado _usuarioLogado;

    public EditarMovimentacaoHandler(
        IMediator mediator,
        IMovimentacaoRepository movimentacaoRepository,
        IUsuarioLogado usuarioLogado
    ) : base(mediator)
    {
        _movimentacaoRepository = movimentacaoRepository;
        _usuarioLogado = usuarioLogado;
    }

    public async Task<ErrorOr<EditarMovimentacaoResponse>> Handle(
        EditarMovimentacaoRequest request,
        CancellationToken cancellationToken)
    {
        if (Validar(request, new EditarMovimentacaoRequest.EditarMovimentacaoRequestValidator()) is var resultado && resultado.Count != 0)
            {
                return resultado;
            }


        var usuarioCadastro = _usuarioLogado.ObterUsuarioId();

        if (string.IsNullOrWhiteSpace(usuarioCadastro))
            return Errors.Application.UsuarioErrors.UsuarioNaoAutenticado;

        var movimentacao = await _movimentacaoRepository
            .BuscarMovimentacaoPorCodigoAsync(request.Codigo, cancellationToken);

        if (movimentacao is null)
            return Errors.Application.MovimentacaoErrors.MovimentacaoNaoEncontrada;

        movimentacao.SetStatus(request.Status);
        movimentacao.SetCodigoEstoqueSolicitado(request.CodigoUsuarioEstoqueSolicitado);
        movimentacao.SetDataAlteracao(DateTime.UtcNow);

        switch (movimentacao.Status)
        {
            case StatusMovimentacao.Novo:
                // movimentação criada
                break;

            case StatusMovimentacao.EmAndamento:
                // movimentação em andamento
                break;

            case StatusMovimentacao.Recusado:
                // movimentação recusada
                break;

            case StatusMovimentacao.Despachado:
                // movimentação despachada
                break;

            case StatusMovimentacao.Entregue:
                // movimentação entregue
                break;

            case StatusMovimentacao.Finalizado:
                // movimentação finalizada
                break;
        }

        _movimentacaoRepository.Atualizar(movimentacao);
        await _movimentacaoRepository.UnitOfWork.CommitAsync(cancellationToken);

        return new EditarMovimentacaoResponse(
            movimentacao.Codigo,
            movimentacao.CodigoUsuarioEstoqueSolicitado,
            movimentacao.Status,
            movimentacao.updated_at
        );
    }
}
