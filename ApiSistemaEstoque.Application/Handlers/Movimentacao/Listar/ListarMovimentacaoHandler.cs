using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ErrorOr;
using MediatR;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.Listar;

public class ListarMovimentacaoHandler : IRequestHandler<ListarMovimentacaoRequest, ErrorOr<IEnumerable<ListarMovimentacaoResponse>>>
{
    private readonly IMovimentacaoRepository _movimentacaoRepository;

    public ListarMovimentacaoHandler(IMovimentacaoRepository movimentacaoRepository)
    {
        _movimentacaoRepository = movimentacaoRepository;
    }

    public async Task<ErrorOr<IEnumerable<ListarMovimentacaoResponse>>> Handle(ListarMovimentacaoRequest request, CancellationToken cancellationToken)
    {
        var movimentacoes = await _movimentacaoRepository.ListarAsync(cancellationToken);

        var response = movimentacoes.Select(movimentacao => new ListarMovimentacaoResponse(
            movimentacao.Codigo,
            movimentacao.Status,
            movimentacao.CodigoTipoMovimentacao,
            movimentacao.CodigoEstoqueSolicitante,
            movimentacao.CodigoUsuarioEstoqueSolicitante,
            movimentacao.CodigoEstoqueSolicitado,
            movimentacao.CodigoUsuarioEstoqueSolicitado,
            movimentacao.CreatedAt,
            movimentacao.updated_at,
            movimentacao.ItensMovimentacao.Select(item => new ListarMovimentacaoItemResponse(
                item.Codigo,
                item.CodigoMovimentacao,
                item.Item,
                item.Quantidade
            )).ToList()
        ));

        return response.ToList();
    }
}
