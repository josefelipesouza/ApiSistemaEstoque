using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using MediatR;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection; // Para IServiceCollection
using Microsoft.AspNetCore.Http;                // Para IHttpContextAccessor
using System.Security.Claims;



namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.Cadastrar;

public class CadastrarCategoriaHandler : BaseHandler, IRequestHandler<CadastrarCategoriaRequest, ErrorOr<CadastrarCategoriaResponse>>
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CadastrarCategoriaHandler(
        IMediator mediator,
        ICategoriaRepository categoriaRepository,
        IHttpContextAccessor httpContextAccessor) : base(mediator)
    {
        _categoriaRepository = categoriaRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ErrorOr<CadastrarCategoriaResponse>> Handle(CadastrarCategoriaRequest request, CancellationToken cancellationToken)
    {
        if (Validar(request, new CadastrarCategoriaRequestValidator()) is var resultado && resultado.Count != 0)
            return resultado;

        var usuarioCadastro = int.Parse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        if (usuarioCadastro == 0)
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
