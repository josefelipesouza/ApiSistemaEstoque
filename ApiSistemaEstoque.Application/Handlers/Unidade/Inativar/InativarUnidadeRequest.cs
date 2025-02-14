using ErrorOr;
using FluentValidation;
using MediatR;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Unidade.Inativar;

public record InativarUnidadeRequest(int Codigo) : IRequest<ErrorOr<bool>>;

public class InativarUnidadeRequestValidator : AbstractValidator<InativarUnidadeRequest>
{
    public InativarUnidadeRequestValidator()
    {
        RuleFor(x => x.Codigo)
            .NotEmpty().WithMessage("O código da unidade deve ser preenchido.")
            .GreaterThan(0).WithMessage("O código da unidade deve ser maior que zero.");
    }
}
