using ErrorOr;
using MediatR;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Estoque.Editar;

public class EditarEstoqueHandler : BaseHandler, IRequestHandler<EditarEstoqueRequest, ErrorOr<EditarEstoqueResponse>>
{
    private readonly IEstoqueRepository _estoqueRepository;

    public EditarEstoqueHandler(
        IMediator mediator,
        IEstoqueRepository estoqueRepository) : base(mediator)
    {
        _estoqueRepository = estoqueRepository;
    }

    public async Task<ErrorOr<EditarEstoqueResponse>> Handle(EditarEstoqueRequest request, CancellationToken cancellationToken)
    {
        if (Validar(request, new EditarEstoqueRequestValidator()) is var resultado && resultado.Any())
            return resultado;

        var estoqueExistente = await _estoqueRepository.BuscarPorCodigoAsync(request.Codigo, cancellationToken);

        if (estoqueExistente is null)
            return Errors.Application.EstoqueErrors.EstoqueNaoEncontrado;

        estoqueExistente.SetDescricao(request.Descricao);
        estoqueExistente.SetLocalizacao(request.Localizacao);
        estoqueExistente.SetResponsavel(request.Responsavel);
        estoqueExistente.SetSuperior(request.Superior);
        estoqueExistente.SetDataAlteracao(DateTime.UtcNow);
        
        _estoqueRepository.Atualizar(estoqueExistente);

        await _estoqueRepository.UnitOfWork.CommitAsync(cancellationToken);

        return new EditarEstoqueResponse(
            estoqueExistente.Codigo,
            estoqueExistente.Descricao,
            estoqueExistente.Localizacao,
            estoqueExistente.Responsavel,
            estoqueExistente.Superior,
            estoqueExistente.CreatedAt,
            estoqueExistente.updated_at,
            estoqueExistente.Inativo
        );
    }
}
