using ErrorOr;
using MediatR;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Item.Listar;

public record ListarItemRequest : IRequest<ErrorOr<IEnumerable<ListarItemResponse>>>;
