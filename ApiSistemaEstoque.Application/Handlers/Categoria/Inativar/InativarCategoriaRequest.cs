using ErrorOr;
using FluentValidation;
using MediatR;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Categoria.Inativar;

public record InativarCategoriaRequest(int Codigo) : IRequest<ErrorOr<bool>>;
public class InativarCategoriaRequestValidator : AbstractValidator<InativarCategoriaRequest>
{
    public InativarCategoriaRequestValidator()
    {
        RuleFor(x => x.Codigo)
            .NotEmpty().WithMessage("O código da solicitação deve ser preenchido.")
            .GreaterThan(0).WithMessage("O código da solicitação deve ser maior que zero.");
    }
}
