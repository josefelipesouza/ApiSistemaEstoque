using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ErrorOr;
using MediatR;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.Buscar;

public class BuscarMovimentacaoHandler : IRequestHandler<BuscarMovimentacaoRequest, ErrorOr<IEnumerable<BuscarMovimentacaoResponse>>>
{
    private readonly IMovimentacaoRepository _movimentacaoRepository;

    public BuscarMovimentacaoHandler(IMovimentacaoRepository movimentacaoRepository)
    {
        _movimentacaoRepository = movimentacaoRepository;
    }

    public async Task<ErrorOr<IEnumerable<BuscarMovimentacaoResponse>>> Handle(BuscarMovimentacaoRequest request, CancellationToken cancellationToken)
    {
        var movimentacoes = await _movimentacaoRepository.BuscarMovimentacaoAsync(
            request.CodigoEstoqueSolicitante ?? 0,
            request.CodigoEstoqueSolicitado ?? 0,
            request.CodigoTipoMovimentacao ?? 0,
            request.DataInicial!.Value,
            request.DataFinal!.Value,
            cancellationToken
        );

        var response = movimentacoes.Select(movimentacao => new BuscarMovimentacaoResponse(
            movimentacao.Codigo,
            movimentacao.Status,
            movimentacao.CodigoTipoMovimentacao,
            movimentacao.CodigoEstoqueSolicitante,
            movimentacao.CodigoUsuarioEstoqueSolicitante,
            movimentacao.CodigoEstoqueSolicitado,
            movimentacao.CodigoUsuarioEstoqueSolicitado,
            movimentacao.CreatedAt,
            movimentacao.updated_at,
            movimentacao.ItensMovimentacao.Select(item => new BuscarMovimentacaoItemResponse(
                item.Codigo,
                item.CodigoMovimentacao,
                item.Item,
                item.Quantidade
            )).ToList()
        ));

        return response.ToList();
    }
}
