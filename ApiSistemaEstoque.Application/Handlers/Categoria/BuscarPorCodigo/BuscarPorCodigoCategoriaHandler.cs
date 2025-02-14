using MediatR;
using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.BuscarPorCodigo;

public class BuscarPorCodigoCategoriaHandler : IRequestHandler<BuscarPorCodigoCategoriaRequest, ErrorOr<BuscarPorCodigoCategoriaResponse>>
{
    private readonly ICategoriaRepository _repository;

    public BuscarPorCodigoCategoriaHandler(ICategoriaRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<ErrorOr<BuscarPorCodigoCategoriaResponse>> Handle(
        BuscarPorCodigoCategoriaRequest request,
        CancellationToken cancellationToken)
    {
        var categoria = await _repository.BuscarPorCodigoAsync(request.Codigo, cancellationToken);

        if (categoria is null)
        {
            return Errors.Application.CategoriaErrors.CategoriaNaoEncontrada;
        }

        var response = new BuscarPorCodigoCategoriaResponse(
            categoria.Codigo,
            categoria.Descricao,
            categoria.Superior,
            categoria.UsuarioCadastro,
            categoria.CreatedAt,
            categoria.updated_at,
            categoria.Inativo!
        );

        return response;
    }
    
}
