using ErrorOr;
using MediatR;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Auth;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Unidade.Cadastrar;

public class CadastrarUnidadeHandler 
    : BaseHandler, IRequestHandler<CadastrarUnidadeRequest, ErrorOr<CadastrarUnidadeResponse>>
{
    private readonly IUnidadeRepository _unidadeRepository;
    private readonly IUsuarioLogado _usuarioLogado;

    public CadastrarUnidadeHandler(
        IMediator mediator,
        IUnidadeRepository unidadeRepository,
        IUsuarioLogado usuarioLogado
    ) : base(mediator)
    {
        _unidadeRepository = unidadeRepository;
        _usuarioLogado = usuarioLogado;
    }

    public async Task<ErrorOr<CadastrarUnidadeResponse>> Handle(
        CadastrarUnidadeRequest request,
        CancellationToken cancellationToken)
    {
        if (Validar(request, new CadastrarUnidadeRequestValidator()) is var resultado
            && resultado.Count != 0)
            return resultado;

        var usuarioCadastro = _usuarioLogado.ObterUsuarioId();

        if (string.IsNullOrWhiteSpace(usuarioCadastro))
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
