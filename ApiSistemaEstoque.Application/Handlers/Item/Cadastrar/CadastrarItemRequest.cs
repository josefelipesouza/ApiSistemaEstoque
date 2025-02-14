using MediatR;
using ErrorOr;
using FluentValidation;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Item.Cadastrar;

public record CadastrarItemRequest (

    string Descricao, 
    int QuantidadeMinima, 
    string Referencia, 
    int CodigoCategoria, 
    int CodigoUnidade 
): IRequest<ErrorOr<CadastrarItemResponse>>;
    public class CadastrarItemRequestValidator : AbstractValidator<CadastrarItemRequest>
    {
        public CadastrarItemRequestValidator()
        {
            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("A descrição é obrigatória.")
                .MaximumLength(100).WithMessage("A descrição deve ter no máximo 100 caracteres.");

            RuleFor(x => x.QuantidadeMinima)
                .GreaterThan(0).WithMessage("A quantidade mínima deve ser maior que zero.");

            RuleFor(x => x.Referencia)
                .NotEmpty().WithMessage("A referência é obrigatória.")
                .MaximumLength(50).WithMessage("A referência deve ter no máximo 50 caracteres.");

            RuleFor(x => x.CodigoCategoria)
                .GreaterThan(0).WithMessage("O código da categoria é obrigatório.");

            RuleFor(x => x.CodigoUnidade)
                .GreaterThan(0).WithMessage("O código da unidade é obrigatório.");
        }
    }

