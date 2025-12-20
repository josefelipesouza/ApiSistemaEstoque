using MediatR;
using ErrorOr;
using FluentValidation;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Estoque.Editar;

public record EditarEstoqueRequest(

        int Codigo,
        string Descricao,
        string Localizacao,
        string Responsavel,
        int Superior
        
): IRequest<ErrorOr<EditarEstoqueResponse>>;

public class EditarEstoqueRequestValidator : AbstractValidator<EditarEstoqueRequest>
{
    public EditarEstoqueRequestValidator()
    {

        RuleFor(x => x.Codigo)
            .NotNull().WithMessage("O código é obrigatório.");

        RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("A descrição é obrigatória.")
                .MaximumLength(100).WithMessage("A descrição deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Localizacao)
            .NotEmpty().WithMessage("A localização é obrigatória.")
            .MaximumLength(500).WithMessage("A localização deve ter no máximo 500 caracteres.");

        RuleFor(x => x.Responsavel)
            .NotNull().WithMessage("O responsável é obrigatório.");

        RuleFor(x => x.Superior)
            .NotNull().WithMessage("O superior é obrigatório.");

    }
}