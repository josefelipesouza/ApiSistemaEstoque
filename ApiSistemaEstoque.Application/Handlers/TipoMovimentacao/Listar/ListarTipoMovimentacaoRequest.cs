using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.TipoMovimentacao.Listar;
using ErrorOr;
using MediatR;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.TipoMovimentacao.Listar;

public record ListarTipoMovimentacaoRequest : IRequest<ErrorOr<IEnumerable<ListarTipoMovimentacaoResponse>>>;
