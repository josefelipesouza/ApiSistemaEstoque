using ErrorOr;
using MediatR;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection; // Para IServiceCollection
using Microsoft.AspNetCore.Http;                // Para IHttpContextAccessor
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;


namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.Editar;

public class EditarCategoriaHandler : BaseHandler, IRequestHandler<EditarCategoriaRequest, ErrorOr<EditarCategoriaResponse>>
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<IdentityUser> _userManager;

    public EditarCategoriaHandler(
        IMediator mediator,
        ICategoriaRepository categoriaRepository,
        IHttpContextAccessor httpContextAccessor,
        UserManager<IdentityUser> userManager) : base(mediator)
    {
        _categoriaRepository = categoriaRepository;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task<ErrorOr<EditarCategoriaResponse>> Handle(EditarCategoriaRequest request, CancellationToken cancellationToken)
    {
        if (Validar(request, new EditarCategoriaRequestValidator()) is var resultado && resultado.Any())
            return resultado;

        var usuarioCadastro = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(usuarioCadastro))
            return Errors.Application.UsuarioErrors.UsuarioNaoAutenticado;

        var categoriaExistente = await _categoriaRepository.BuscarPorCodigoAsync(request.Codigo, cancellationToken);

        if (categoriaExistente is null)
            return Errors.Application.CategoriaErrors.CategoriaNaoEncontrada;

        categoriaExistente.SetDescricao(request.Descricao);
        categoriaExistente.SetSuperior(request.Superior);
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
