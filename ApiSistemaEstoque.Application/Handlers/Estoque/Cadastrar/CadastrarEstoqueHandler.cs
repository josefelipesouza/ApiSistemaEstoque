using ErrorOr;
using MediatR;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Auth;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Estoque.Cadastrar;

public class CadastrarEstoqueHandler
    : BaseHandler, IRequestHandler<CadastrarEstoqueRequest, ErrorOr<CadastrarEstoqueResponse>>
{
    private readonly IEstoqueRepository _estoqueRepository;
    private readonly IUsuarioLogado _usuarioLogado;

    public CadastrarEstoqueHandler(
        IMediator mediator,
        IEstoqueRepository estoqueRepository,
        IUsuarioLogado usuarioLogado
    ) : base(mediator)
    {
        _estoqueRepository = estoqueRepository;
        _usuarioLogado = usuarioLogado;
    }

    public async Task<ErrorOr<CadastrarEstoqueResponse>> Handle(
        CadastrarEstoqueRequest request,
        CancellationToken cancellationToken)
    {
        if (Validar(request, new CadastrarEstoqueRequestValidator()) is var resultado
            && resultado.Any())
            return resultado;

        var usuarioCadastro = _usuarioLogado.ObterUsuarioId();

        if (string.IsNullOrWhiteSpace(usuarioCadastro))
            return Errors.Application.UsuarioErrors.UsuarioNaoAutenticado;

        int? superior = request.Superior == 0 ? null : request.Superior;

        var novoEstoque = new Domain.Entities.Estoque(
            request.Descricao,
            usuarioCadastro,
            request.Localizacao,
            request.Responsavel,
            superior
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
            novoEstoque.Inativo
        );
    }
}
