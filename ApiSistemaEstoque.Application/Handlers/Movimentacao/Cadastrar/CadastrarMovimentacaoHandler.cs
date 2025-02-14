using MediatR;
using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using System.Security.Claims;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.Cadastrar;

public class CadastrarMovimentacaoHandler : BaseHandler, IRequestHandler<CadastrarMovimentacaoRequest, ErrorOr<CadastrarMovimentacaoResponse>>
{
    private readonly IMovimentacaoRepository _movimentacaoRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CadastrarMovimentacaoHandler(
        IMediator mediator,
        IMovimentacaoRepository movimentacaoRepository,
        IHttpContextAccessor httpContextAccessor) : base(mediator)
    {
        _movimentacaoRepository = movimentacaoRepository;
        _httpContextAccessor = httpContextAccessor;
    }


    public async Task<ErrorOr<CadastrarMovimentacaoResponse>> Handle(CadastrarMovimentacaoRequest request, CancellationToken cancellationToken)
    {
        if (Validar(request, new CadastrarMovimentacaoRequestValidator()) is var resultado && resultado.Count != 0)
            return resultado;

        var usuarioCadastro = int.Parse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        if (usuarioCadastro == 0)
            return Errors.Application.UsuarioErrors.UsuarioNaoAutenticado;   

        // Criação da movimentação
        var movimentacao = new Domain.Entities.Movimentacao(
            request.CodigoTipoMovimentacao,
            request.CodigoEstoqueSolicitante,
            request.CodigoUsuarioEstoqueSolicitante,
            request.CodigoEstoqueSolicitado ?? 0
        );

        // Adicionando a movimentação ao repositório
        await _movimentacaoRepository.AdicionarAsync(movimentacao, cancellationToken);
        await _movimentacaoRepository.UnitOfWork.CommitAsync(cancellationToken);

        foreach (var item in request.Itens)
        {
            var itemMovimentacao = new Domain.Entities.ItemMovimentacao(
                movimentacao.Codigo,
                item.CodigoItem,
                item.Quantidade
            );

            // Adicionar o item ao repositório (se for necessário)
            await _movimentacaoRepository.AdicionarItemAsync(itemMovimentacao, cancellationToken);
        }
        // Commit dos itens
        await _movimentacaoRepository.UnitOfWork.CommitAsync(cancellationToken);

        return new CadastrarMovimentacaoResponse(
            movimentacao.Codigo,
            movimentacao.Status,
            movimentacao.CodigoTipoMovimentacao,
            movimentacao.CodigoEstoqueSolicitante,
            movimentacao.CodigoUsuarioEstoqueSolicitante,
            movimentacao.CodigoEstoqueSolicitado,
            movimentacao.CodigoUsuarioEstoqueSolicitado,
            movimentacao.CreatedAt
        );
    }


}
