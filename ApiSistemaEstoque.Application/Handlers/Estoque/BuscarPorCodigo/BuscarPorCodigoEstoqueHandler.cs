using MediatR;
using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Estoque.BuscarPorCodigo;

public class BuscarPorCodigoEstoqueHandler 
    : IRequestHandler<BuscarPorCodigoEstoqueRequest, ErrorOr<BuscarPorCodigoEstoqueResponse>>
{
    private readonly IEstoqueRepository _repository;

    public BuscarPorCodigoEstoqueHandler(IEstoqueRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<BuscarPorCodigoEstoqueResponse>> Handle(
        BuscarPorCodigoEstoqueRequest request,
        CancellationToken cancellationToken)
    {
        var estoque = await _repository.BuscarPorCodigoAsync(request.Codigo, cancellationToken);

        if (estoque is null)
            return Errors.Application.EstoqueErrors.EstoqueNaoEncontrado;

        return new BuscarPorCodigoEstoqueResponse(
            estoque.Codigo,
            estoque.Descricao,
            estoque.Localizacao,
            estoque.Responsavel,
            estoque.Superior, // int?
            estoque.CreatedAt,
            estoque.updated_at,
            estoque.Inativo
        );
    }
}
