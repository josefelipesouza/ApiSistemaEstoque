using ErrorOr;
using MediatR;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.ItemEstoque.Listar;

public record ListarItemEstoqueRequest : IRequest<ErrorOr<IEnumerable<ListarItemEstoqueResponse>>>;
