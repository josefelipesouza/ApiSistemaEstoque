using ErrorOr;
using MediatR;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Auth;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.Editar;

public class EditarCategoriaHandler 
    : BaseHandler, IRequestHandler<EditarCategoriaRequest, ErrorOr<EditarCategoriaResponse>>
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IUsuarioLogado _usuarioLogado;

    public EditarCategoriaHandler(
        IMediator mediator,
        ICategoriaRepository categoriaRepository,
        IUsuarioLogado usuarioLogado
    ) : base(mediator)
    {
        _categoriaRepository = categoriaRepository;
        _usuarioLogado = usuarioLogado;
    }

    public async Task<ErrorOr<EditarCategoriaResponse>> Handle(
        EditarCategoriaRequest request,
        CancellationToken cancellationToken)
    {
        if (Validar(request, new EditarCategoriaRequestValidator()) is var resultado 
            && resultado.Any())
            return resultado;

        var usuarioCadastro = _usuarioLogado.ObterUsuarioId();

        if (string.IsNullOrWhiteSpace(usuarioCadastro))
            return Errors.Application.UsuarioErrors.UsuarioNaoAutenticado;

        var categoriaExistente = await _categoriaRepository
            .BuscarPorCodigoAsync(request.Codigo, cancellationToken);

        if (categoriaExistente is null)
            return Errors.Application.CategoriaErrors.CategoriaNaoEncontrada;

        // =============================
        // REGRA DE NEGÓCIO: 0 → null
        // =============================
        int? superiorTratado = request.Superior == 0
            ? null
            : request.Superior;

        // =============================
        // Validação: categoria superior deve existir
        // =============================
        if (superiorTratado.HasValue)
        {
            int codigoSuperior = superiorTratado.Value;

            var categoriaSuperior = await _categoriaRepository
                .BuscarPorCodigoAsync(codigoSuperior, cancellationToken);

            if (categoriaSuperior is null)
                return Errors.Application.CategoriaErrors.CategoriaNaoEncontrada;
        }


        categoriaExistente.SetDescricao(request.Descricao);
        categoriaExistente.SetSuperior(superiorTratado);
        categoriaExistente.SetUsuarioCadastro(usuarioCadastro);
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
