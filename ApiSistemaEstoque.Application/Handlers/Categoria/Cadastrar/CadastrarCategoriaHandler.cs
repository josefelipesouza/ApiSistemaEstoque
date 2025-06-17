using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using MediatR;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection; // Para IServiceCollection
using Microsoft.AspNetCore.Http;                // Para IHttpContextAccessor
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;



namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.Cadastrar;

public class CadastrarCategoriaHandler : BaseHandler, IRequestHandler<CadastrarCategoriaRequest, ErrorOr<CadastrarCategoriaResponse>>
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<IdentityUser> _userManager;

    public CadastrarCategoriaHandler(
        IMediator mediator,
        ICategoriaRepository categoriaRepository,
        IHttpContextAccessor httpContextAccessor, 
        UserManager<IdentityUser> userManager) : base(mediator)
    {
        _categoriaRepository = categoriaRepository;
        _httpContextAccessor = httpContextAccessor;
         _userManager = userManager;
    }

    public async Task<ErrorOr<CadastrarCategoriaResponse>> Handle(CadastrarCategoriaRequest request, CancellationToken cancellationToken)
    {
        if (Validar(request, new CadastrarCategoriaRequestValidator()) is var resultado && resultado.Count != 0)
            return resultado;

        var usuarioCadastroId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);


        var usuario = await _userManager.FindByIdAsync(usuarioCadastroId);
        if (usuario == null)
            return Errors.Application.UsuarioErrors.UsuarioNaoAutenticado;
        /*
        if (string.IsNullOrWhiteSpace(usuarioCadastroId))
            return Errors.Application.UsuarioErrors.UsuarioNaoAutenticado;
            */


        var novaCategoria = new Domain.Entities.Categoria(
            request.Descricao,
            request.Superior,
            usuarioCadastroId
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
