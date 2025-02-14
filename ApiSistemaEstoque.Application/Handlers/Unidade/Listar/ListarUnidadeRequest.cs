using ErrorOr;
using MediatR;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Unidade.Listar;

public record ListarUnidadeRequest : IRequest<ErrorOr<IEnumerable<ListarUnidadeResponse>>>;
