using ErrorOr;
using MediatR;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.Editar;

public class EditarMovimentacaoHandler : IRequestHandler<EditarMovimentacaoRequest, ErrorOr<EditarMovimentacaoResponse>>
{
    private readonly IMovimentacaoRepository _movimentacaoRepository;

    public EditarMovimentacaoHandler(IMovimentacaoRepository movimentacaoRepository)
    {
        _movimentacaoRepository = movimentacaoRepository;
    }

    public async Task<ErrorOr<EditarMovimentacaoResponse>> Handle(EditarMovimentacaoRequest request, CancellationToken cancellationToken)
    {
        var validator = new EditarMovimentacaoRequest.EditarMovimentacaoRequestValidator();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
            return validationResult.Errors
                .Select(e => Error.Validation(e.PropertyName, e.ErrorMessage))
                .ToList();

        var movimentacao = await _movimentacaoRepository.BuscarMovimentacaoPorCodigoAsync(request.Codigo, cancellationToken);

        if (movimentacao is null)
            return Errors.Application.MovimentacaoErrors.MovimentacaoNaoEncontrada;

        movimentacao.SetStatus(request.Status);
        movimentacao.SetCodigoEstoqueSolicitado(request.CodigoUsuarioEstoqueSolicitado);
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
