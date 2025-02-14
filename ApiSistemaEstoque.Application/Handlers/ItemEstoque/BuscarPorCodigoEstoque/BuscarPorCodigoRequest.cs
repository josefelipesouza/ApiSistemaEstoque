using MediatR;
using ErrorOr;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.ItemEstoque.BuscarPorCodigoEstoque;

public record BuscarPorCodigoRequest(int CodigoEstoque) : IRequest<ErrorOr<BuscarPorCodigoResponse>>;
