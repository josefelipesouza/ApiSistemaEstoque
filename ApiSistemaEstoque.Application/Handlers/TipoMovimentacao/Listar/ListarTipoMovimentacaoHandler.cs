using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ErrorOr;
using MediatR;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.TipoMovimentacao.Listar;

public class ListarTipoMovimentacaoHandler : BaseHandler, IRequestHandler<ListarTipoMovimentacaoRequest, ErrorOr<IEnumerable<ListarTipoMovimentacaoResponse>>>
{
    private readonly ITipoMovimentacaoRepository _tipoMovimentacaoRepository;

    public ListarTipoMovimentacaoHandler(
        IMediator mediator,
        ITipoMovimentacaoRepository tipoMovimentacaoRepository) : base(mediator)
    {
        _tipoMovimentacaoRepository = tipoMovimentacaoRepository;
    }

    public async Task<ErrorOr<IEnumerable<ListarTipoMovimentacaoResponse>>> Handle(ListarTipoMovimentacaoRequest request, CancellationToken cancellationToken)
    {
        var tipos = await _tipoMovimentacaoRepository.ListarAsync(cancellationToken);

        var response = tipos.Select(tipo => new ListarTipoMovimentacaoResponse(
            tipo.Codigo,
            tipo.Descricao,
            tipo.Tipo,
            tipo.UsuarioCadastro,
            tipo.CreatedAt,
            tipo.UpdatedAt,
            tipo.Inativo
        ));

        return response.ToList();
    }
}
