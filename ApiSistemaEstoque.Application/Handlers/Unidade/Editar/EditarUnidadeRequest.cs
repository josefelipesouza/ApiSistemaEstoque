using MediatR;
using ErrorOr;
using FluentValidation;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Unidade.Editar;

public class EditarUnidadeRequest : IRequest<ErrorOr<EditarUnidadeResponse>>
{
    public int Codigo { get; set; }
    public required string Descricao { get; set; }

    public class EditarUnidadeRequestValidator : AbstractValidator<EditarUnidadeRequest>
    {
        public EditarUnidadeRequestValidator()
        {
            RuleFor(x => x.Codigo)
                .NotNull().WithMessage("O código é obrigatório.");

            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("A descrição é obrigatória.")
                .MaximumLength(100).WithMessage("A descrição deve ter no máximo 100 caracteres.");
        }
    }
}
