using MediatR;
using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Unidade.BuscarPorCodigo;

public class BuscarPorCodigoUnidadeHandler : IRequestHandler<BuscarPorCodigoUnidadeRequest, ErrorOr<BuscarPorCodigoUnidadeResponse>>
{
    private readonly IUnidadeRepository _repository;

    public BuscarPorCodigoUnidadeHandler(IUnidadeRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<BuscarPorCodigoUnidadeResponse>> Handle(
        BuscarPorCodigoUnidadeRequest request,
        CancellationToken cancellationToken)
    {
        var unidade = await _repository.BuscarPorCodigoAsync(request.Codigo, cancellationToken);

        if (unidade is null)
        {
            return Errors.Application.UnidadeErrors.UnidadeNaoEncontrada;
        }

        var response = new BuscarPorCodigoUnidadeResponse(
            unidade.Codigo,
            unidade.Descricao,
            unidade.UsuarioCadastro,
            unidade.CreatedAt,
            unidade.updated_at,
            unidade.Inativo!
        );

        return response;
    }
}
