using MediatR;
using ErrorOr;
using FluentValidation;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.ItemEstoque.Cadastrar;

public record CadastrarItemEstoqueRequest(

    int CodigoItem,
    int CodigoEstoque,
    int Quantidade
)  : IRequest<ErrorOr<CadastrarItemEstoqueResponse>>;
    public class CadastrarItemEstoqueRequestValidator : AbstractValidator<CadastrarItemEstoqueRequest>
    {
        public CadastrarItemEstoqueRequestValidator()
        {
            RuleFor(x => x.CodigoItem)
                .GreaterThan(0).WithMessage("O código do item é obrigatório.");

            RuleFor(x => x.CodigoEstoque)
                .GreaterThan(0).WithMessage("O código do estoque é obrigatório.");

            RuleFor(x => x.Quantidade)
                .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero.");
        }
    }

