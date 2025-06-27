using MediatR;
using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.TipoMovimentacao.BuscarPorCodigo;

public class BuscarPorCodigoTipoMovimentacaoHandler 
    : IRequestHandler<BuscarPorCodigoTipoMovimentacaoRequest, ErrorOr<BuscarPorCodigoTipoMovimentacaoResponse>>
{
    private readonly ITipoMovimentacaoRepository _repository;

    public BuscarPorCodigoTipoMovimentacaoHandler(ITipoMovimentacaoRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<BuscarPorCodigoTipoMovimentacaoResponse>> Handle(
        BuscarPorCodigoTipoMovimentacaoRequest request,
        CancellationToken cancellationToken)
    {
        var tipoMovimentacao = await _repository.BuscarPorCodigoAsync(request.Codigo, cancellationToken);

        if (tipoMovimentacao is null)
        {
            return Errors.Application.TipoMovimentacaoErrors.TipoMovimentacaoNaoEncontrada;
        }

        var response = new BuscarPorCodigoTipoMovimentacaoResponse(
            tipoMovimentacao.Codigo,
            tipoMovimentacao.Tipo,
            tipoMovimentacao.Descricao,
            tipoMovimentacao.UsuarioCadastro,
            tipoMovimentacao.CreatedAt,
            tipoMovimentacao.updated_at,
            tipoMovimentacao.Inativo
        );

        return response;
    }
}
