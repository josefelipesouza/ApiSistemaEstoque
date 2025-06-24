using MediatR;
using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection; // Para IServiceCollection
using Microsoft.AspNetCore.Http;                // Para IHttpContextAccessor
using System.Security.Claims;



namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Item.Cadastrar;

public class CadastrarItemHandler : BaseHandler, IRequestHandler<CadastrarItemRequest, ErrorOr<CadastrarItemResponse>>
{
    private readonly IItemRepository _itemRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CadastrarItemHandler(
        IMediator mediator,
        IItemRepository itemRepository,
        IHttpContextAccessor httpContextAccessor) : base(mediator)
    {
        _itemRepository = itemRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ErrorOr<CadastrarItemResponse>> Handle(CadastrarItemRequest request, CancellationToken cancellationToken)
    {

        if (Validar(request, new CadastrarItemRequestValidator()) is var resultado && resultado.Count != 0)
            return resultado;    

        var usuarioCadastro = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(usuarioCadastro))
            return Errors.Application.UsuarioErrors.UsuarioNaoAutenticado;

        var novoItem = new Domain.Entities.Item(
            request.Descricao,
            usuarioCadastro,
            request.QuantidadeMinima,
            request.Referencia,
            request.CodigoCategoria,
            request.CodigoUnidade
        );

        await _itemRepository.AdicionarAsync(novoItem, cancellationToken);
        await _itemRepository.UnitOfWork.CommitAsync(cancellationToken);

        return new CadastrarItemResponse(
            novoItem.Codigo,
            novoItem.Descricao,
            novoItem.QuantidadeMinima,
            novoItem.Referencia,
            novoItem.CodigoCategoria,
            novoItem.CodigoUnidade,
            novoItem.UsuarioCadastro,
            novoItem.CreatedAt,
            novoItem.updated_at,
            novoItem.Inativo
        );
    }
}
