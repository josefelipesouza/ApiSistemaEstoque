using ErrorOr;
using FluentValidation;
using MediatR;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Item.Editar;

public class EditarItemRequest : IRequest<ErrorOr<EditarItemResponse>>
{
    public int Codigo { get; set; }
    public required string Descricao { get; set; }
    public int QuantidadeMinima { get; set; }
    public required string Referencia { get; set; }
    public int CodigoCategoria { get; set; }
    public int CodigoUnidade { get; set; }

    public class EditarItemRequestValidator : AbstractValidator<EditarItemRequest>
    {
        public EditarItemRequestValidator()
        {
            RuleFor(x => x.Codigo)
                .NotNull().WithMessage("O código é obrigatório.");

            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("A descrição é obrigatória.")
                .MaximumLength(100).WithMessage("A descrição deve ter no máximo 100 caracteres.");

            RuleFor(x => x.QuantidadeMinima)
                .GreaterThanOrEqualTo(0).WithMessage("A quantidade mínima deve ser igual ou maior que zero.");

            RuleFor(x => x.Referencia)
                .NotEmpty().WithMessage("A referência é obrigatória.")
                .MaximumLength(50).WithMessage("A referência deve ter no máximo 50 caracteres.");

            RuleFor(x => x.CodigoCategoria)
                .NotNull().WithMessage("O código da categoria é obrigatório.");

            RuleFor(x => x.CodigoUnidade)
                .NotNull().WithMessage("O código da unidade é obrigatório.");
        }
    }
}
