using MediatR;
using ErrorOr;
using FluentValidation;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.Editar;

public class EditarCategoriaRequest : IRequest<ErrorOr<EditarCategoriaResponse>>
{
    public int Codigo { get; set; }
    public required string Descricao { get; set; }
    public int Superior { get; set; }
    public string UsuarioCadastro { get; set; }
}

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

        RuleFor(x => x.UsuarioCadastro)
            .NotNull().WithMessage("O código é obrigatório.");
    }
}
