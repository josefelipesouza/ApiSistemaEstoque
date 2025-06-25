using MediatR;
using ErrorOr;
using FluentValidation;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.TipoMovimentacao.Cadastrar;

public record CadastrarTipoMovimentacaoRequest(
    IEnumerable<TipoBaseMovimentacao> Tipo,
    string Descricao
) : IRequest<ErrorOr<CadastrarTipoMovimentacaoResponse>>;

public class CadastrarTipoMovimentacaoRequestValidator : AbstractValidator<CadastrarTipoMovimentacaoRequest>
{
    public CadastrarTipoMovimentacaoRequestValidator()
    {
        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("A descrição é obrigatória.")
            .MaximumLength(100).WithMessage("A descrição deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Tipo)
            .NotEmpty().WithMessage("Pelo menos um tipo de movimentação deve ser informado.");
    }
}
