using MediatR;
using ErrorOr;
using FluentValidation;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.Editar;

public record EditarCategoriaRequest(
    int Codigo,
    string Descricao,
    int? Superior
) : IRequest<ErrorOr<EditarCategoriaResponse>>;


public class EditarCategoriaRequestValidator : AbstractValidator<EditarCategoriaRequest>
{
    public EditarCategoriaRequestValidator()
    {
        RuleFor(x => x.Codigo)
            .NotNull().WithMessage("O código é obrigatório.");

        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("A descrição é obrigatória.")
            .MaximumLength(100).WithMessage("A descrição deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Superior)
            .NotNull().WithMessage("O superior é obrigatório.");

    }
}
