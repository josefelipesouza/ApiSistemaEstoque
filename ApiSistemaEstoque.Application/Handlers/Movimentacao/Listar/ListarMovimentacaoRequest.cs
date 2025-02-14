using ErrorOr;
using MediatR;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.Listar;

public record ListarMovimentacaoRequest : IRequest<ErrorOr<IEnumerable<ListarMovimentacaoResponse>>>;
