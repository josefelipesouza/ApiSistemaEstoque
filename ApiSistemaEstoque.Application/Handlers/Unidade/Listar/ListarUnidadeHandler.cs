using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ErrorOr;
using MediatR;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Unidade.Listar;

public class ListarUnidadeHandler : BaseHandler, IRequestHandler<ListarUnidadeRequest, ErrorOr<IEnumerable<ListarUnidadeResponse>>>
{
    private readonly IUnidadeRepository _unidadeRepository;

    public ListarUnidadeHandler(IMediator mediator,
        IUnidadeRepository unidadeRepository) : base(mediator)
    {
        _unidadeRepository = unidadeRepository;
         
    }

    public async Task<ErrorOr<IEnumerable<ListarUnidadeResponse>>> Handle(ListarUnidadeRequest request, CancellationToken cancellationToken)
    {
        var unidades = await _unidadeRepository.ListarAsync(cancellationToken);

        var response = unidades.Select(unidade => new ListarUnidadeResponse(
            unidade.Codigo,
            unidade.Descricao,
            unidade.UsuarioCadastro,
            unidade.CreatedAt,
            unidade.updated_at,
            unidade.Inativo
        ));

        return response.ToList();
    }
}
