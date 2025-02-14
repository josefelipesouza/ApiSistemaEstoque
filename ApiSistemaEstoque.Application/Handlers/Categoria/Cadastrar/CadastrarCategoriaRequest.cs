using MediatR;
using ErrorOr;
using FluentValidation;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.Cadastrar;

public record CadastrarCategoriaRequest (

    string Descricao,
    int Superior,
    int UsuarioCadastro
) : IRequest<ErrorOr<CadastrarCategoriaResponse>>;

public class CadastrarCategoriaRequestValidator : AbstractValidator<CadastrarCategoriaRequest>
{
    public CadastrarCategoriaRequestValidator()
    {
        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("A descrição é obrigatória.")
            .MaximumLength(100).WithMessage("A descrição deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Superior)
            .GreaterThan(0).WithMessage("O código do superior deve ser um valor positivo.");

        RuleFor(x => x.UsuarioCadastro)
            .NotNull().WithMessage("O usuário de cadastro é obrigatório.");

    }
}
