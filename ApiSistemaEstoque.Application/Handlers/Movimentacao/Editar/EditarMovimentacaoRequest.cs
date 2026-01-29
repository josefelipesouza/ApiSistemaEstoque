using ErrorOr;
using FluentValidation;
using MediatR;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Movimentacao.Editar;

public class EditarMovimentacaoRequest : IRequest<ErrorOr<EditarMovimentacaoResponse>>
{
    public int Codigo { get; set; }
    public string PlacaVeiculo {get; set;}
    public StatusMovimentacao Status { get; set; }

    public class EditarMovimentacaoRequestValidator : AbstractValidator<EditarMovimentacaoRequest>
    {
        public EditarMovimentacaoRequestValidator()
        {

            RuleFor(x => x.Codigo)
                .NotNull().WithMessage("O código é obrigatório.")
                .GreaterThan(0).WithMessage("O código deve ser maior que zero.");

            RuleFor(x => x.PlacaVeiculo)
                .NotEmpty()
                .WithMessage("A placa do veículo é obrigatória quando a movimentação for despachada.")
                .MaximumLength(7)
                .WithMessage("A placa deve ter no máximo 7 caracteres.")
                .When(x => x.Status == StatusMovimentacao.Despachado);

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("O status informado é inválido.");
        }
    }
}
