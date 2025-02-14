using MediatR;
using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.BuscarPorCodigo;

public class BuscarPorCodigoMovimentacaoHandler 
    : IRequestHandler<BuscarPorCodigoMovimentacaoRequest, ErrorOr<BuscarPorCodigoMovimentacaoResponse>>
{
    private readonly IMovimentacaoRepository _repository;

    public BuscarPorCodigoMovimentacaoHandler(IMovimentacaoRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<BuscarPorCodigoMovimentacaoResponse>> Handle(
        BuscarPorCodigoMovimentacaoRequest request,
        CancellationToken cancellationToken)
    {
        var movimentacao = await _repository.BuscarMovimentacaoPorCodigoAsync(request.Codigo, cancellationToken);

        if (movimentacao is null)
        {
            return Errors.Application.MovimentacaoErrors.MovimentacaoNaoEncontrada;
        }

        var response = new BuscarPorCodigoMovimentacaoResponse(
            movimentacao.Codigo,
            movimentacao.Status,
            movimentacao.CodigoTipoMovimentacao,
            movimentacao.CodigoEstoqueSolicitante,
            movimentacao.CodigoUsuarioEstoqueSolicitante,
            movimentacao.CodigoEstoqueSolicitado,
            movimentacao.CodigoUsuarioEstoqueSolicitado,
            movimentacao.CreatedAt,
            movimentacao.updated_at,
            movimentacao.ItensMovimentacao.Select(i => new ItemMovimentacaoResponse(
                i.Codigo,
                i.CodigoMovimentacao,
                i.Item,
                i.Quantidade)).ToList()
        );

        return response;
    }
}
