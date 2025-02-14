using MediatR;
using ErrorOr;
using FluentValidation;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.BuscarPorCodigo;

public record BuscarPorCodigoMovimentacaoRequest : IRequest<ErrorOr<BuscarPorCodigoMovimentacaoResponse>>
{
    public int Codigo { get; set; }

    public class Validator : AbstractValidator<BuscarPorCodigoMovimentacaoRequest>
    {
        public Validator()
        {
            RuleFor(x => x.Codigo)
                .GreaterThan(0).WithMessage("O código deve ser maior que zero.")
                .NotNull().WithMessage("O código é obrigatório.");
        }
    }
}
