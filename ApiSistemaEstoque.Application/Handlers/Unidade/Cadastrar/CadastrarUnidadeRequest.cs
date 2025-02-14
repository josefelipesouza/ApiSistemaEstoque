using MediatR;
using ErrorOr;
using FluentValidation;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers.Unidade.Cadastrar;

public record CadastrarUnidadeRequest(

    string Descricao 
) : IRequest<ErrorOr<CadastrarUnidadeResponse>>;

public class CadastrarUnidadeRequestValidator : AbstractValidator<CadastrarUnidadeRequest>
{
    public CadastrarUnidadeRequestValidator()
    {
        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("A descrição é obrigatória.")
            .MaximumLength(100).WithMessage("A descrição deve ter no máximo 100 caracteres.");

    }
}
