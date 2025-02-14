using ErrorOr;
using MediatR;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.Listar;

public record ListarCategoriaRequest : IRequest<ErrorOr<IEnumerable<ListarCategoriaResponse>>>;
