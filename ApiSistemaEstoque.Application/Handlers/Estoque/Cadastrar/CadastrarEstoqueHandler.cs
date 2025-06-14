using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using MediatR;
using System.Security.Claims;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;
using Microsoft.Extensions.DependencyInjection; // Para IServiceCollection
using Microsoft.AspNetCore.Http;                // Para IHttpContextAccessor
using System.Security.Claims;


namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Estoque.Cadastrar;

public class CadastrarEstoqueHandler : BaseHandler, IRequestHandler<CadastrarEstoqueRequest, ErrorOr<CadastrarEstoqueResponse>>
{
    private readonly IEstoqueRepository _estoqueRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CadastrarEstoqueHandler(
        IMediator mediator,
        IEstoqueRepository estoqueRepository,
        IHttpContextAccessor httpContextAccessor) : base(mediator)
    {
        _estoqueRepository = estoqueRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ErrorOr<CadastrarEstoqueResponse>> Handle(CadastrarEstoqueRequest request, CancellationToken cancellationToken)
    {
        if (Validar(request, new CadastrarEstoqueRequestValidator()) is var resultado && resultado.Count != 0)
            return resultado;

        var usuarioCadastro = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);


        if (string.IsNullOrWhiteSpace(usuarioCadastro))
    return Errors.Application.UsuarioErrors.UsuarioNaoAutenticado;    
        
        var novoEstoque = new Domain.Entities.Estoque(
            request.Descricao,
            usuarioCadastro,
            request.Localizacao,
            request.Responsavel,
            request.Superior
        );

        await _estoqueRepository.AdicionarAsync(novoEstoque, cancellationToken);
        await _estoqueRepository.UnitOfWork.CommitAsync(cancellationToken);

        return new CadastrarEstoqueResponse(
            novoEstoque.Codigo,
            novoEstoque.Descricao,
            novoEstoque.Localizacao,
            novoEstoque.Responsavel,
            novoEstoque.Superior,
            novoEstoque.CreatedAt,
            novoEstoque.updated_at,
            novoEstoque.Inativo.FirstOrDefault() == Status.Inativo
        );
    }
}
