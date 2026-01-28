using MediatR;
using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Auth;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.Cadastrar;

public class CadastrarMovimentacaoHandler
    : BaseHandler, IRequestHandler<CadastrarMovimentacaoRequest, ErrorOr<CadastrarMovimentacaoResponse>>
{
    private readonly IMovimentacaoRepository _movimentacaoRepository;
    private readonly IItemEstoqueRepository _itemEstoqueRepository;
    private readonly IUsuarioEstoqueRepository _usuarioEstoqueRepository;
    private readonly IUsuarioLogado _usuarioLogado;

    public CadastrarMovimentacaoHandler(
        IMediator mediator,
        IMovimentacaoRepository movimentacaoRepository,
        IItemEstoqueRepository itemEstoqueRepository,
        IUsuarioEstoqueRepository usuarioEstoqueRepository,
        IUsuarioLogado usuarioLogado
    ) : base(mediator)
    {
        _movimentacaoRepository = movimentacaoRepository;
        _itemEstoqueRepository = itemEstoqueRepository;
        _usuarioEstoqueRepository = usuarioEstoqueRepository;
        _usuarioLogado = usuarioLogado;
    }

    public async Task<ErrorOr<CadastrarMovimentacaoResponse>> Handle(
        CadastrarMovimentacaoRequest request,
        CancellationToken cancellationToken)
    {
        // ======================================================
        // Validação
        // ======================================================
        var erros = Validar(request, new CadastrarMovimentacaoRequestValidator());
        if (erros.Count != 0)
            return erros;

        var usuarioCadastro = _usuarioLogado.ObterUsuarioId();
        if (string.IsNullOrWhiteSpace(usuarioCadastro))
            return Errors.Application.UsuarioErrors.UsuarioNaoAutenticado;
        // ======================================================
        // Obter estoque solicitante do usuário
        // ======================================================
        var CodigoEstoqueSolicitante = await _usuarioEstoqueRepository.ObterCodigoEstoquePorUsuarioAsync(usuarioCadastro, cancellationToken);

        if (CodigoEstoqueSolicitante == null)
            return Errors.Application.UsuarioErrors.UsuarioNaoVinculadoAEstoque;
    
        // ======================================================
        // Criação da movimentação
        // ======================================================
        var movimentacao = new Domain.Entities.Movimentacao(
            request.CodigoTipoMovimentacao,
            CodigoEstoqueSolicitante.Value,
            usuarioCadastro,
            request.CodigoEstoqueSolicitado ?? 0
        );

        await _movimentacaoRepository.AdicionarAsync(movimentacao, cancellationToken);
        await _movimentacaoRepository.UnitOfWork.CommitAsync(cancellationToken);

        var tipoMovimentacao = (TipoBaseMovimentacao)request.CodigoTipoMovimentacao;

        // ======================================================
        // Processamento dos itens
        // ======================================================
        foreach (var item in request.Itens)
        {
            // -------------------------------
            // Item da movimentação
            // -------------------------------
            var itemMovimentacao = new Domain.Entities.ItemMovimentacao(
                movimentacao.Codigo,
                item.CodigoItem,
                item.Quantidade
            );

            await _movimentacaoRepository.AdicionarItemAsync(itemMovimentacao, cancellationToken);

            var itemEstoqueAtual =
                await _itemEstoqueRepository.BuscarPorCodigoEstoqueItem(
                    CodigoEstoqueSolicitante.Value,
                    item.CodigoItem,
                    cancellationToken);

            switch (tipoMovimentacao)
            {
                // ==================================================
                // ENTRADA / CORREÇÃO ENTRADA
                // ==================================================
                case TipoBaseMovimentacao.Entrada:
                case TipoBaseMovimentacao.CorreçãoEntrada:
                {
                    if (itemEstoqueAtual is null)
                    {
                        var novoItemEstoque = new Domain.Entities.ItemEstoque(
                            item.CodigoItem,
                            CodigoEstoqueSolicitante.Value,
                            item.Quantidade
                        );
                        // novo item no estoque
                        await _itemEstoqueRepository.AdicionarAsync(
                            novoItemEstoque,
                            cancellationToken
                        );
                    }
                    else
                    {
                        var novaQuantidade =
                            itemEstoqueAtual.Quantidade + item.Quantidade;

                        await _itemEstoqueRepository.AtualizarQuantidadeAsync(
                            CodigoEstoqueSolicitante.Value,
                            item.CodigoItem,
                            novaQuantidade,
                            cancellationToken
                        );
                    }

                    break;
                }

                // ==================================================
                // SAÍDA / CORREÇÃO SAÍDA
                // ==================================================
                case TipoBaseMovimentacao.Saida:
                case TipoBaseMovimentacao.CorreçãoSaida:
                {
                    if (itemEstoqueAtual is null)
                        return Errors.Application.ItemEstoqueErrors.ItemEstoqueNaoEncontrado;

                    if (itemEstoqueAtual.Quantidade < item.Quantidade)
                        return Errors.Application.ItemEstoqueErrors.ItemEstoqueQuantidadeInsuficiente;

                    var novaQuantidade =
                        itemEstoqueAtual.Quantidade - item.Quantidade;

                    await _itemEstoqueRepository.AtualizarQuantidadeAsync(
                        CodigoEstoqueSolicitante.Value,
                        item.CodigoItem,
                        novaQuantidade,
                        cancellationToken
                    );

                    break;
                }

                // ==================================================
                // OUTROS TIPOS (Transferência, etc.)
                // ==================================================
                default:
                    break;
            }
        }

        // ======================================================
        // Commit final
        // ======================================================
        await _movimentacaoRepository.UnitOfWork.CommitAsync(cancellationToken);
        await _itemEstoqueRepository.UnitOfWork.CommitAsync(cancellationToken);

        // ======================================================
        // Response
        // ======================================================
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
