using ErrorOr;
using FluentValidation;
using MediatR;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.TipoMovimentacao.Inativar;

public record InativarTipoMovimentacaoRequest(int Codigo) : IRequest<ErrorOr<bool>>;

public class InativarTipoMovimentacaoRequestValidator : AbstractValidator<InativarTipoMovimentacaoRequest>
{
    public InativarTipoMovimentacaoRequestValidator()
    {
        RuleFor(x => x.Codigo)
            .NotEmpty().WithMessage("O código da solicitação deve ser preenchido.")
            .GreaterThan(0).WithMessage("O código da solicitação deve ser maior que zero.");
    }
}

