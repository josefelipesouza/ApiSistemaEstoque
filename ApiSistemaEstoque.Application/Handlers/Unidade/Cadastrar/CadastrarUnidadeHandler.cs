using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using MediatR;
using System.Security.Claims;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Unidade.Cadastrar;

public class CadastrarUnidadeHandler : BaseHandler, IRequestHandler<CadastrarUnidadeRequest, ErrorOr<CadastrarUnidadeResponse>>
{
    private readonly IUnidadeRepository _unidadeRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CadastrarUnidadeHandler(
        IMediator mediator,
        IUnidadeRepository unidadeRepository,
        IHttpContextAccessor httpContextAccessor) : base(mediator)
    {
        _unidadeRepository = unidadeRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ErrorOr<CadastrarUnidadeResponse>> Handle(CadastrarUnidadeRequest request, CancellationToken cancellationToken)
    {
        if (Validar(request, new CadastrarUnidadeRequestValidator()) is var resultado && resultado.Count != 0)
            return resultado;

        var usuarioCadastro = int.Parse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        if (usuarioCadastro == 0)
            return Errors.Application.UsuarioErrors.UsuarioNaoAutenticado;

        var novaUnidade = new Domain.Entities.Unidade(
            request.Descricao,
            usuarioCadastro
        );

        await _unidadeRepository.AdicionarAsync(novaUnidade, cancellationToken);
        await _unidadeRepository.UnitOfWork.CommitAsync(cancellationToken);

        return new CadastrarUnidadeResponse(
            novaUnidade.Codigo,
            novaUnidade.Descricao,
            novaUnidade.UsuarioCadastro,
            novaUnidade.CreatedAt,
            novaUnidade.updated_at,
            novaUnidade.Inativo
        );
    }
}
