using MediatR;
using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection; // Para IServiceCollection
using Microsoft.AspNetCore.Http;                // Para IHttpContextAccessor
using System.Security.Claims;



namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.ItemEstoque.Cadastrar;

public class CadastrarItemEstoqueHandler : BaseHandler, IRequestHandler<CadastrarItemEstoqueRequest, ErrorOr<CadastrarItemEstoqueResponse>>
{
    private readonly IItemEstoqueRepository _itemEstoqueRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CadastrarItemEstoqueHandler(
        IMediator mediator,
        IItemEstoqueRepository itemEstoqueRepository,
        IHttpContextAccessor httpContextAccessor) : base(mediator)
    {
        _itemEstoqueRepository = itemEstoqueRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ErrorOr<CadastrarItemEstoqueResponse>> Handle(CadastrarItemEstoqueRequest request, CancellationToken cancellationToken)
    {

        if (Validar(request, new CadastrarItemEstoqueRequestValidator()) is var resultado && resultado.Count != 0)
            return resultado;

        var usuarioCadastro = int.Parse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        if (usuarioCadastro == 0)
            return Errors.Application.UsuarioErrors.UsuarioNaoAutenticado;

        var novoItemEstoque = new Domain.Entities.ItemEstoque(
            request.CodigoItem,
            request.CodigoEstoque,
            request.Quantidade
        );

        await _itemEstoqueRepository.AdicionarAsync(novoItemEstoque, cancellationToken);
        await _itemEstoqueRepository.UnitOfWork.CommitAsync(cancellationToken);

        return new CadastrarItemEstoqueResponse(
            novoItemEstoque.Codigo,
            novoItemEstoque.CodigoItem,
            novoItemEstoque.CodigoEstoque,
            novoItemEstoque.Quantidade,
            novoItemEstoque.CreatedAt,
            novoItemEstoque.updated_at
        );
    }
}
