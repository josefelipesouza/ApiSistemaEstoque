using ErrorOr;
using MediatR;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.Editar;

public class EditarCategoriaHandler : BaseHandler, IRequestHandler<EditarCategoriaRequest, ErrorOr<EditarCategoriaResponse>>
{
    private readonly ICategoriaRepository _categoriaRepository;

    public EditarCategoriaHandler(
        IMediator mediator,
        ICategoriaRepository categoriaRepository) : base(mediator)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<ErrorOr<EditarCategoriaResponse>> Handle(EditarCategoriaRequest request, CancellationToken cancellationToken)
    {
        if (Validar(request, new EditarCategoriaRequestValidator()) is var resultado && resultado.Any())
            return resultado;

        var categoriaExistente = await _categoriaRepository.BuscarPorCodigoAsync(request.Codigo, cancellationToken);

        if (categoriaExistente is null)
            return Errors.Application.CategoriaErrors.CategoriaNaoEncontrada;

        categoriaExistente.SetDescricao(request.Descricao);
        categoriaExistente.SetSuperior(request.Superior);
        categoriaExistente.SetUsuarioCadastro(request.UsuarioCadastro);
        categoriaExistente.SetDataAlteracao(DateTime.UtcNow);

        _categoriaRepository.Atualizar(categoriaExistente);

        await _categoriaRepository.UnitOfWork.CommitAsync(cancellationToken);

        return new EditarCategoriaResponse(
            categoriaExistente.Codigo,
            categoriaExistente.Descricao,
            categoriaExistente.Superior,
            categoriaExistente.UsuarioCadastro,
            categoriaExistente.CreatedAt,
            categoriaExistente.updated_at,
            categoriaExistente.Inativo            

        );
    }
}
