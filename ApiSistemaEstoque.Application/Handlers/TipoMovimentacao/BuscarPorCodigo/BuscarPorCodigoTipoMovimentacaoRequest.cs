using MediatR;
using ErrorOr;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.TipoMovimentacao.BuscarPorCodigo;

public record BuscarPorCodigoTipoMovimentacaoRequest(int Codigo) 
    : IRequest<ErrorOr<BuscarPorCodigoTipoMovimentacaoResponse>>;
