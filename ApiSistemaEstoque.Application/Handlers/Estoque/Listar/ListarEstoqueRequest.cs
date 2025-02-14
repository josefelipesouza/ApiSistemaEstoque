using ErrorOr;
using MediatR;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Estoque.Listar;

public record ListarEstoqueRequest : IRequest<ErrorOr<IEnumerable<ListarEstoqueResponse>>>;
