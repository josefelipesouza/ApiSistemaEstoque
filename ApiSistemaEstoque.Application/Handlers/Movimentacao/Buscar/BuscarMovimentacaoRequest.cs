using ErrorOr;
using FluentValidation;
using MediatR;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.Buscar;

/// <summary>
/// Request para buscar movimentações.
/// </summary>
public record BuscarMovimentacaoRequest(
    int? CodigoTipoMovimentacao,
    int? CodigoEstoqueSolicitante,
    int? CodigoEstoqueSolicitado,
    DateTime? DataInicial,
    DateTime? DataFinal
) : IRequest<ErrorOr<IEnumerable<BuscarMovimentacaoResponse>>>
{
    public class Validator : AbstractValidator<BuscarMovimentacaoRequest>
    {
        public Validator()
        {
            RuleFor(x => x.DataInicial)
                .NotNull().WithMessage("A data inicial é obrigatória.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("A data inicial não pode ser no futuro.");

            RuleFor(x => x.DataFinal)
                .NotNull().WithMessage("A data final é obrigatória.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("A data final não pode ser no futuro.");

            RuleFor(x => new { x.DataInicial, x.DataFinal })
                .Must(d => d.DataInicial.HasValue && d.DataFinal.HasValue && d.DataInicial <= d.DataFinal)
                .WithMessage("A data inicial deve ser menor ou igual à data final.");

            RuleFor(x => x.CodigoTipoMovimentacao)
                .GreaterThan(0).When(x => x.CodigoTipoMovimentacao.HasValue)
                .WithMessage("O código do tipo de movimentação deve ser maior que zero.");

            RuleFor(x => x.CodigoEstoqueSolicitante)
                .GreaterThan(0).When(x => x.CodigoEstoqueSolicitante.HasValue)
                .WithMessage("O código do estoque solicitante deve ser maior que zero.");

            RuleFor(x => x.CodigoEstoqueSolicitado)
                .GreaterThan(0).When(x => x.CodigoEstoqueSolicitado.HasValue)
                .WithMessage("O código do estoque solicitado deve ser maior que zero.");

        }
    }
}
