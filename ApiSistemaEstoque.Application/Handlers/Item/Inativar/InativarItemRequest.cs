using ErrorOr;
using FluentValidation;
using MediatR;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Item.Inativar;

public record InativarItemRequest(int Codigo) : IRequest<ErrorOr<bool>>;

public class InativarItemRequestValidator : AbstractValidator<InativarItemRequest>
{
    public InativarItemRequestValidator()
    {
        RuleFor(x => x.Codigo)
            .NotEmpty().WithMessage("O código do item deve ser preenchido.")
            .GreaterThan(0).WithMessage("O código do item deve ser maior que zero.");
    }
}
