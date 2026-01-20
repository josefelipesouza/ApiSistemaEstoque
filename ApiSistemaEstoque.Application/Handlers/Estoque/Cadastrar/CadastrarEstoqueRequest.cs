using MediatR;
using ErrorOr;
using FluentValidation;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Estoque.Cadastrar;

public record CadastrarEstoqueRequest(
    
        string Descricao,
        string Localizacao,
        string Responsavel,
        int? Superior
        
): IRequest<ErrorOr<CadastrarEstoqueResponse>>;

public class CadastrarEstoqueRequestValidator 
    : AbstractValidator<CadastrarEstoqueRequest>
{
    public CadastrarEstoqueRequestValidator()
    {
        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("A descrição é obrigatória.")
            .MaximumLength(100).WithMessage("A descrição deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Localizacao)
            .NotEmpty().WithMessage("A localização é obrigatória.")
            .MaximumLength(100).WithMessage("A localização deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Responsavel)
            .NotEmpty().WithMessage("O responsável é obrigatório.");

        // Aceita null, 0 ou maior que 0
        RuleFor(x => x.Superior)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Superior.HasValue)
            .WithMessage("O código do superior deve ser zero ou um valor positivo.");
    }
}