using ErrorOr;
using FluentValidation;
using MediatR;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Estoque.Inativar;

public record InativarEstoqueRequest(int Codigo) : IRequest<ErrorOr<bool>>;
public class InativarEstoqueRequestValidator : AbstractValidator<InativarEstoqueRequest>
{
    public InativarEstoqueRequestValidator()
    {
        RuleFor(x => x.Codigo)
            .NotEmpty().WithMessage("O código da solicitação deve ser preenchido.")
            .GreaterThan(0).WithMessage("O código da solicitação deve ser maior que zero.");
    }
}
