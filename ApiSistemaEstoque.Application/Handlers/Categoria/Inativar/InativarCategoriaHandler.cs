using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using MediatR;
using static ApiSistemaEstoque.ApiSistemaEstoque.Application.Errors.Application;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.Inativar;

public class InativarCategoriaHandler
    : BaseHandler, IRequestHandler<InativarCategoriaRequest, ErrorOr<bool>>
{
    private readonly ICategoriaRepository _categoriaRepository;

    public InativarCategoriaHandler(
        ICategoriaRepository categoriaRepository,
        IMediator mediator) : base(mediator)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<ErrorOr<bool>> Handle(
        InativarCategoriaRequest request,
        CancellationToken cancellationToken)
    {
        var validationErrors = Validar(request, new InativarCategoriaRequestValidator());
        if (validationErrors.Any())
        {
            return validationErrors;
        }

        var categoria = await _categoriaRepository.BuscarPorCodigoAsync(request.Codigo, cancellationToken);

        if (categoria is null)
            return CategoriaErrors.CategoriaNaoEncontrada;

        _categoriaRepository.Inativar(categoria);

        await _categoriaRepository.UnitOfWork.CommitAsync(cancellationToken);

        return true;
    }
}
