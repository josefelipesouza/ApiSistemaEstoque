using MediatR;
using ErrorOr;
using FluentValidation;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.Cadastrar;

public record CadastrarCategoriaRequest (

    string Descricao,
    int Superior
) : IRequest<ErrorOr<CadastrarCategoriaResponse>>;

public class CadastrarCategoriaRequestValidator : AbstractValidator<CadastrarCategoriaRequest>
{
    public CadastrarCategoriaRequestValidator()
    {
        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("A descrição é obrigatória.")
            .MaximumLength(100).WithMessage("A descrição deve ter no máximo 100 caracteres.");

        // Alterado para permitir 0 (que será tratado como null no handler)
        RuleFor(x => x.Superior)
            .GreaterThanOrEqualTo(0).WithMessage("O código do superior deve ser um valor positivo ou zero.");
    }
}
