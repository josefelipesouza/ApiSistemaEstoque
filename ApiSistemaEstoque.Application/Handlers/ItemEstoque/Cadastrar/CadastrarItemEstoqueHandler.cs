using MediatR;
using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Auth;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.ItemEstoque.Cadastrar;

public class CadastrarItemEstoqueHandler 
    : BaseHandler, IRequestHandler<CadastrarItemEstoqueRequest, ErrorOr<CadastrarItemEstoqueResponse>>
{
    private readonly IItemEstoqueRepository _itemEstoqueRepository;
    private readonly IUsuarioLogado _usuarioLogado;

    public CadastrarItemEstoqueHandler(
        IMediator mediator,
        IItemEstoqueRepository itemEstoqueRepository,
        IUsuarioLogado usuarioLogado
    ) : base(mediator)
    {
        _itemEstoqueRepository = itemEstoqueRepository;
        _usuarioLogado = usuarioLogado;
    }

    public async Task<ErrorOr<CadastrarItemEstoqueResponse>> Handle(
        CadastrarItemEstoqueRequest request,
        CancellationToken cancellationToken)
    {
        if (Validar(request, new CadastrarItemEstoqueRequestValidator()) is var resultado 
            && resultado.Count != 0)
            return resultado;

        var usuarioCadastro = _usuarioLogado.ObterUsuarioId();

        if (string.IsNullOrWhiteSpace(usuarioCadastro))
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
