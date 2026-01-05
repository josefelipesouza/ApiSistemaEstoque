using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Auth;
using MediatR;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.Cadastrar;

public class CadastrarCategoriaHandler 
    : BaseHandler, IRequestHandler<CadastrarCategoriaRequest, ErrorOr<CadastrarCategoriaResponse>>
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IUsuarioLogado _usuarioLogado;

    public CadastrarCategoriaHandler(
        IMediator mediator,
        ICategoriaRepository categoriaRepository,
        IUsuarioLogado usuarioLogado
    ) : base(mediator)
    {
        _categoriaRepository = categoriaRepository;
        _usuarioLogado = usuarioLogado;
    }

    public async Task<ErrorOr<CadastrarCategoriaResponse>> Handle(
        CadastrarCategoriaRequest request,
        CancellationToken cancellationToken)
    {
        if (Validar(request, new CadastrarCategoriaRequestValidator()) is var resultado 
            && resultado.Count != 0)
            return resultado;

        var usuarioCadastro = _usuarioLogado.ObterUsuarioId();

        if (string.IsNullOrWhiteSpace(usuarioCadastro))
            return Errors.Application.UsuarioErrors.UsuarioNaoAutenticado;

        var novaCategoria = new Domain.Entities.Categoria(
            request.Descricao,
            request.Superior,
            usuarioCadastro
        );

        await _categoriaRepository.AdicionarAsync(novaCategoria, cancellationToken);
        await _categoriaRepository.UnitOfWork.CommitAsync(cancellationToken);

        return new CadastrarCategoriaResponse(
            novaCategoria.Codigo,
            novaCategoria.Descricao,
            novaCategoria.Superior,
            novaCategoria.UsuarioCadastro,
            novaCategoria.CreatedAt,
            novaCategoria.updated_at,
            novaCategoria.Inativo
        );
    }
}
