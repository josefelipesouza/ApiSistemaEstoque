using ErrorOr;
using MediatR;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using static ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.Editar.EditarMovimentacaoRequest;
using System.Security.Claims;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.Editar;

public class EditarMovimentacaoHandler : BaseHandler, IRequestHandler<EditarMovimentacaoRequest, ErrorOr<EditarMovimentacaoResponse>>
{
    private readonly IMovimentacaoRepository _movimentacaoRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<IdentityUser> _userManager;

    public EditarMovimentacaoHandler(
         IMediator mediator,
        IMovimentacaoRepository movimentacaoRepository,
         IHttpContextAccessor httpContextAccessor,
        UserManager<IdentityUser> userManager
        ) : base(mediator)
    {
        _movimentacaoRepository = movimentacaoRepository;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task<ErrorOr<EditarMovimentacaoResponse>> Handle(EditarMovimentacaoRequest request, CancellationToken cancellationToken)
    {

        if (Validar(request, new EditarMovimentacaoRequestValidator()) is var resultado && resultado.Count != 0)
            return resultado;

        var usuarioCadastro = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(usuarioCadastro))
            return Errors.Application.UsuarioErrors.UsuarioNaoAutenticado;

        var movimentacao = await _movimentacaoRepository.BuscarMovimentacaoPorCodigoAsync(request.Codigo, cancellationToken);

        if (movimentacao is null)
            return Errors.Application.MovimentacaoErrors.MovimentacaoNaoEncontrada;



        movimentacao.SetStatus(request.Status);
        movimentacao.SetCodigoEstoqueSolicitado(request.CodigoUsuarioEstoqueSolicitado);
        switch (movimentacao.Status)
        {
            case StatusMovimentacao.Novo: 
                Console.WriteLine("Movimentação criada. Aguardando processamento.");
                // Lógica adicional: notificar responsável, por exemplo
                break;

            case StatusMovimentacao.EmAndamento:
                Console.WriteLine("Movimentação está em andamento.");
                // Ex: travar edição de campos
                break;

            case StatusMovimentacao.Recusado:
                Console.WriteLine("Movimentação recusada.");
                // Ex: logar o motivo, enviar e-mail
                break;

            case StatusMovimentacao.Despachado:
                Console.WriteLine("Movimentação despachada para entrega.");
                // Ex: atualizar rastreio
                break;

            case StatusMovimentacao.Entregue:
                Console.WriteLine("Movimentação entregue com sucesso.");
                // Ex: permitir avaliação
                break;

            case StatusMovimentacao.Finalizado:
                Console.WriteLine("Movimentação finalizada.");
                // Ex: gerar comprovante ou relatório
                break;

            default:
                Console.WriteLine("Status desconhecido.");
                // Ex: log de erro
                break;
        }
        movimentacao.SetDataAlteracao(DateTime.UtcNow);

        _movimentacaoRepository.Atualizar(movimentacao);
        await _movimentacaoRepository.UnitOfWork.CommitAsync(cancellationToken);

        return new EditarMovimentacaoResponse(
            movimentacao.Codigo,
            movimentacao.CodigoUsuarioEstoqueSolicitado,
            movimentacao.Status,
            movimentacao.updated_at
        );
    }
}
