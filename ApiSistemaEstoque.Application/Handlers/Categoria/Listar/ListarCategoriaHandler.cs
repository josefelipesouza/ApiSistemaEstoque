using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ErrorOr;
using MediatR;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.Listar;

public class ListarCategoriaHandler : BaseHandler, IRequestHandler<ListarCategoriaRequest, ErrorOr<IEnumerable<ListarCategoriaResponse>>>
{
    private readonly ICategoriaRepository _categoriaRepository;

    public ListarCategoriaHandler(
        IMediator mediator,
        ICategoriaRepository categoriaRepository) : base(mediator)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<ErrorOr<IEnumerable<ListarCategoriaResponse>>> Handle(ListarCategoriaRequest request, CancellationToken cancellationToken)
    {
        var categorias = await _categoriaRepository.ListarAsync(cancellationToken);

        var response = categorias.Select(categoria => new ListarCategoriaResponse(
            categoria.Codigo,
            categoria.Descricao,
            categoria.Superior,
            categoria.UsuarioCadastro,
            categoria.CreatedAt,
            categoria.updated_at,
            categoria.Inativo
        ));

        return response.ToList();
    }
}
