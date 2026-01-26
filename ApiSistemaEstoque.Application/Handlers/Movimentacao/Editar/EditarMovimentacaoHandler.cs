using ErrorOr;
using MediatR;
using MovimentacaoEntity = ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities.Movimentacao;
using TransporteEntity = ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities.Transporte;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Auth;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain;
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
        switch ((TipoBaseMovimentacao)movimentacao.CodigoTipoMovimentacao)
        {
            case TipoBaseMovimentacao.Solicitacao:
                await ProcessarSolicitacaoAsync(movimentacao, request, cancellationToken);
                break;

            case TipoBaseMovimentacao.Devolucao:
                break;
        }


        movimentacao.SetStatus(request.Status);
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
    private async Task<ErrorOr<Success>> ProcessarSolicitacaoAsync(MovimentacaoEntity movimentacao, EditarMovimentacaoRequest request, CancellationToken cancellationToken)
    {
        // ==================================================
        // 🚚 DESPACHADO
        // ==================================================
        if (request.Status == StatusMovimentacao.Despachado)
        {
            foreach (var item in movimentacao.ItensMovimentacao)
            {
                var itemEstoqueRemetente = await _itemEstoqueRepository
                    .BuscarPorCodigoEstoqueItem(
                        movimentacao.CodigoEstoqueSolicitado,
                        item.Item,
                        cancellationToken
                    );

                if (itemEstoqueRemetente is null)
                    return Errors.Application.ItemEstoqueErrors.ItemEstoqueNaoEncontrado;

                if (item.Quantidade <= 0)
                    return Errors.Application.ItemEstoqueErrors.ItemEstoqueQuantidadeInvalida;

                if (itemEstoqueRemetente.Quantidade < item.Quantidade)
                    return Errors.Application.ItemEstoqueErrors.ItemEstoqueQuantidadeInsuficiente;

                //Débito do estoque remetente
                var novaQuantidade = itemEstoqueRemetente.Quantidade - item.Quantidade;

                await _itemEstoqueRepository.AtualizarQuantidadeAsync(
                    movimentacao.CodigoEstoqueSolicitado,
                    item.Item,
                    novaQuantidade,
                    cancellationToken
                );

                itemEstoqueRemetente.SetDataAlteracao(DateTime.UtcNow);

                //Cria transporte
                var transporte = new TransporteEntity(
                    request.PlacaVeiculo,
                    movimentacao.Codigo,
                    item.Item,
                    item.Quantidade
                );

                await _transporteRepository.AdicionarAsync(transporte, cancellationToken);
            }

            // Persiste estoque + transportes
            await _itemEstoqueRepository.UnitOfWork.CommitAsync(cancellationToken);
            return Result.Success;
        }

        // ==================================================
        // 📦 ENTREGUE
        // ==================================================
        if (request.Status == StatusMovimentacao.Entregue)
        {
            var transportes = await _transporteRepository
                .BuscarPorCodigoMovimentacaoAsync(
                    movimentacao.Codigo,
                    cancellationToken
                );

            foreach (var transporte in transportes)
            {
                var itemEstoqueDestino = await _itemEstoqueRepository
                    .BuscarPorCodigoEstoqueItem(
                        movimentacao.CodigoEstoqueSolicitante,
                        transporte.CodigoItem,
                        cancellationToken
                    );

                if (itemEstoqueDestino is null)
                {
                    //Cria item no estoque destino
                    itemEstoqueDestino = new Domain.Entities.ItemEstoque(
                        transporte.CodigoItem,
                        movimentacao.CodigoEstoqueSolicitante,
                        transporte.Quantidade
                    );

                    await _itemEstoqueRepository
                        .AdicionarAsync(itemEstoqueDestino, cancellationToken);
                }
                else
                {
                    //Entrada no estoque destino
                    var novaQuantidade = itemEstoqueDestino.Quantidade + transporte.Quantidade;

                    await _itemEstoqueRepository.AtualizarQuantidadeAsync(
                    movimentacao.CodigoEstoqueSolicitante,
                    transporte.CodigoItem,
                    novaQuantidade,
                    cancellationToken
                );

                    itemEstoqueDestino.SetDataAlteracao(DateTime.UtcNow);
                }

                //Finaliza transporte
                await _transporteRepository.AtualizarDataEntregaAsync(movimentacao.Codigo, cancellationToken);

            }

            // Commit único (estoque + transporte)
            await _itemEstoqueRepository.UnitOfWork.CommitAsync(cancellationToken);
            return Result.Success;
        }

        return Errors.Application.MovimentacaoErrors.TransacaoInvalida;

    }



    // ===============================
    // 🔄 Devolução
    // ===============================
/*
    private async Task ProcessarDevolucaoAsync(Movimentacao movimentacao, EditarMovimentacaoRequest request, CancellationToken cancellationToken)
    {
        // 🚚 DESPACHADO → sai do estoque colicitado e vai para transporte
        if (request.Status == StatusMovimentacao.Despachado)
        {
            foreach (var item in movimentacao.ItensMovimentacao)
            {
                // 🔻 Debita do estoque solicitante
                var QuantidadeDisponivel = await _itemEstoqueRepository
                    .BuscarPorCodigoEstoqueItem(
                        movimentacao.CodigoEstoqueSolicitante,
                        item.Item,
                        cancellationToken
                    );

                 if (item.Quantidade <= 0)
                    throw new DomainException("Quantidade solicitada inválida");

                if (QuantidadeDisponivel < item.Quantidade)
                    throw new DomainException("Estoque insuficiente");    

                var QuantidadeAtualizada = QuantidadeDisponivel - item.Quantidade;

                await _itemEstoqueRepository.AtualizarQuantidadeAsync(
                            movimentacao.CodigoEstoqueSolicitante,
                            item.Item,
                            QuantidadeAtualizada,
                            cancellationToken
                        );

                await _itemEstoqueRepository.UnitOfWork.CommitAsync(cancellationToken);

                // 🚚 Cria transporte
                var transporte = new Domain.Entities.Transporte(
                    request.PlacaVeiculo,
                    movimentacao.Codigo,
                    item.Item,// seria .codigo ?
                    item.Quantidade
                );

                _transporteRepository.Adicionar(transporte);

                await _transporteRepository.UnitOfWork.CommitAsync(cancellationToken);
            }

            return;
        }

        // 📦 ENTREGUE// 
        if (request.Status == StatusMovimentacao.Entregue)
        {

            _transporteRepository.AtualizarDataEntregaAsync(movimentacao.Codigo);

            await _transporteRepository.UnitOfWork.CommitAsync(cancellationToken);
           
        }
    }
    */
}
