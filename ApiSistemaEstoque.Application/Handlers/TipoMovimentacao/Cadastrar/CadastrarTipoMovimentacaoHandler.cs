using ErrorOr;
using MediatR;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using  ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.TipoMovimentacao.Cadastrar;

public class CadastrarTipoMovimentacaoHandler : BaseHandler, IRequestHandler<CadastrarTipoMovimentacaoRequest, ErrorOr<CadastrarTipoMovimentacaoResponse>>
{
    private readonly ITipoMovimentacaoRepository _tipoMovimentacaoRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<IdentityUser> _userManager;

    public CadastrarTipoMovimentacaoHandler(
        IMediator mediator,
        ITipoMovimentacaoRepository tipoMovimentacaoRepository,
        IHttpContextAccessor httpContextAccessor,
        UserManager<IdentityUser> userManager) : base(mediator)
    {
        _tipoMovimentacaoRepository = tipoMovimentacaoRepository;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task<ErrorOr<CadastrarTipoMovimentacaoResponse>> Handle(CadastrarTipoMovimentacaoRequest request, CancellationToken cancellationToken)
    {
        if (Validar(request, new CadastrarTipoMovimentacaoRequestValidator()) is var resultado && resultado.Count != 0)
            return resultado;

        var usuarioCadastro = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(usuarioCadastro))
            return Errors.Application.UsuarioErrors.UsuarioNaoAutenticado;

        var nova = new Domain.Entities.TipoMovimentacao(
            request.Tipo,
            request.Descricao,
            usuarioCadastro
        );

        await _tipoMovimentacaoRepository.AdicionarAsync(nova, cancellationToken);
        await _tipoMovimentacaoRepository.UnitOfWork.CommitAsync(cancellationToken);

        return new CadastrarTipoMovimentacaoResponse(
            nova.Codigo,
            nova.Tipo,
            nova.Descricao!,
            nova.UsuarioCadastro,
            nova.CreatedAt,
            nova.updated_at,
            nova.Inativo
        );
    }
}
