using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ErrorOr;
using MediatR;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Estoque.Listar;

public class ListarEstoqueHandler : BaseHandler, IRequestHandler<ListarEstoqueRequest, ErrorOr<IEnumerable<ListarEstoqueResponse>>>
{
    private readonly IEstoqueRepository _estoqueRepository;

    public ListarEstoqueHandler(
        IMediator mediator,
        IEstoqueRepository estoqueRepository) : base(mediator)
    {
        _estoqueRepository = estoqueRepository;
    }

    public async Task<ErrorOr<IEnumerable<ListarEstoqueResponse>>> Handle(ListarEstoqueRequest request, CancellationToken cancellationToken)
    {
    
        var estoques = await _estoqueRepository.ListarAsync(cancellationToken);

        var response = estoques.Select(estoque => new ListarEstoqueResponse(
            estoque.Codigo,
            estoque.Descricao,
            estoque.Localizacao,
            estoque.Responsavel,
            estoque.Superior,
            estoque.CreatedAt,
            estoque.updated_at,
            estoque.Inativo
        ));

        return response.ToList();
    }
}
