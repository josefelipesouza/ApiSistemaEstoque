using ErrorOr;
using MediatR;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Auth;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.Editar;

public class EditarMovimentacaoHandler
    : BaseHandler, IRequestHandler<EditarMovimentacaoRequest, ErrorOr<EditarMovimentacaoResponse>>
{
    private readonly IMovimentacaoRepository _movimentacaoRepository;
    private readonly ITransporteRepository _transporteRepository;
    private readonly IItemEstoqueRepository _itemEstoqueRepository;
    private readonly IUsuarioLogado _usuarioLogado;

    public EditarMovimentacaoHandler(
        IMediator mediator,
        IMovimentacaoRepository movimentacaoRepository,
        ITransporteRepository transporteRepository,
        IItemEstoqueRepository itemEstoqueRepository,
        IUsuarioLogado usuarioLogado
    ) : base(mediator)
    {
        _movimentacaoRepository = movimentacaoRepository;
        _transporteRepository = transporteRepository;
        _itemEstoqueRepository = itemEstoqueRepository;
        _usuarioLogado = usuarioLogado;
    }

    public async Task<ErrorOr<EditarMovimentacaoResponse>> Handle(
        EditarMovimentacaoRequest request,
        CancellationToken cancellationToken)
    {
        // 🔎 Validação
        var erros = Validar(request, new EditarMovimentacaoRequest.EditarMovimentacaoRequestValidator());
        if (erros.Count != 0)
            return erros;

        var usuarioId = _usuarioLogado.ObterUsuarioId();
        if (string.IsNullOrWhiteSpace(usuarioId))
            return Errors.Application.UsuarioErrors.UsuarioNaoAutenticado;

        var movimentacao = await _movimentacaoRepository
            .BuscarMovimentacaoPorCodigoAsync(request.Codigo, cancellationToken);

        if (movimentacao is null)
            return Errors.Application.MovimentacaoErrors.MovimentacaoNaoEncontrada;

        // 🔒 Estados imutáveis
        if (movimentacao.Status is StatusMovimentacao.Entregue
            or StatusMovimentacao.Finalizado)
        {
            return Errors.Application.MovimentacaoErrors.MovimentacaoNaoPodeSerAlterada;
        }

        // 🔁 Validação de transição
        if (!TransicaoPermitida(movimentacao.Status, request.Status))
            return Errors.Application.MovimentacaoErrors.TransicaoStatusInvalida;

        // 📦 Regras por tipo de movimentação
        switch (movimentacao.CodigoTipoMovimentacao)
        {
            case TipoMovimentacaoEnum.Solicitacao:
                await ProcessarSolicitacaoAsync(movimentacao, request, cancellationToken);
                break;

            case TipoMovimentacaoEnum.Devolucao:
                await ProcessarDevolucaoAsync(movimentacao, request, cancellationToken);
                break;
        }

        movimentacao.SetStatus(request.Status);
        movimentacao.SetDataAlteracao(DateTime.UtcNow);

        _movimentacaoRepository.Atualizar(movimentacao);
        await _movimentacaoRepository.UnitOfWork.CommitAsync(cancellationToken);

        return new EditarMovimentacaoResponse(
            movimentacao.Codigo,
            movimentacao.Status,
            movimentacao.updated_at
        );
    }

    // ===============================
    // 🔁 Regras de Transição
    // ===============================
    private static bool TransicaoPermitida(
        StatusMovimentacao atual,
        StatusMovimentacao novo)
    {
        return atual switch
        {
            StatusMovimentacao.Novo =>
                novo is StatusMovimentacao.EmAndamento or StatusMovimentacao.Recusado,

            StatusMovimentacao.EmAndamento =>
                novo is StatusMovimentacao.Recusado or StatusMovimentacao.Despachado,

            StatusMovimentacao.Despachado =>
                novo is StatusMovimentacao.Entregue,

            _ => false
        };
    }

    // ===============================
    // 📦 Solicitação
    // ===============================
    private async Task ProcessarSolicitacaoAsync(
        Domain.Entities.Movimentacao movimentacao,
        EditarMovimentacaoRequest request,
        CancellationToken cancellationToken)
    {
        // 🚚 DESPACHADO → sai da matriz e vai para transporte
        if (request.Status == StatusMovimentacao.Despachado)
        {
            foreach (var item in movimentacao.ItensMovimentacao)
            {
                // 🔻 Debita do estoque solicitado (matriz)
                var estoqueItemMatriz = await _itemEstoqueRepository
                    .BuscarAsync(
                        movimentacao.CodigoEstoqueSolicitado,
                        item.Item,
                        cancellationToken
                    );

                estoqueItemMatriz.Debitar(item.Quantidade);

                // 🚚 Cria transporte
                var transporte = new Domain.Entities.Transporte(
                    request.PlacaVeiculo,
                    movimentacao.Codigo,
                    item.Item,
                    item.Quantidade
                );

                _transporteRepository.Adicionar(transporte);
            }

            return;
        }

        // 📦 ENTREGUE → sai do transporte e entra no estoque solicitante
        if (request.Status == StatusMovimentacao.Entregue)
        {
            foreach (var item in movimentacao.ItensMovimentacao)
            {
                // ➕ Entra no estoque solicitante
                var estoqueItemSolicitante = await _itemEstoqueRepository
                    .BuscarAsync(
                        movimentacao.CodigoEstoqueSolicitante,
                        item.Item,
                        cancellationToken
                    );

                estoqueItemSolicitante.Creditar(item.Quantidade);
            }

            // ❌ Remove todos os itens do transporte dessa movimentação
            _transporteRepository.RemoverPorMovimentacao(movimentacao.Codigo);
        }
    }

    // ===============================
    // 🔄 Devolução
    // ===============================
    private async Task ProcessarDevolucaoAsync(
        Domain.Entities.Movimentacao movimentacao,
        EditarMovimentacaoRequest request,
        CancellationToken cancellationToken)
    {
        if (request.Status == StatusMovimentacao.Despachado)
        {
            foreach (var item in movimentacao.ItensMovimentacao)
            {
                // 🔻 Sai do estoque solicitante
                var estoqueItem = await _itemEstoqueRepository
                    .BuscarAsync(movimentacao.CodigoEstoqueSolicitante, item.Item, cancellationToken);

                estoqueItem.Debitar(item.Quantidade);

                var transporte = new Domain.Entities.Transporte(
                    request.PlacaVeiculo,
                    movimentacao.Codigo,
                    item.Item,
                    item.Quantidade
                );

                _transporteRepository.Adicionar(transporte);
            }
        }

        if (request.Status == StatusMovimentacao.Entregue)
        {
            foreach (var item in movimentacao.ItensMovimentacao)
            {
                // ➕ Entra no estoque solicitado (matriz)
                var estoqueItem = await _itemEstoqueRepository
                    .BuscarAsync(movimentacao.CodigoEstoqueSolicitado, item.Item, cancellationToken);

                estoqueItem.Creditar(item.Quantidade);

                _transporteRepository.RemoverPorMovimentacao(movimentacao.Codigo);
            }
        }
    }
}
