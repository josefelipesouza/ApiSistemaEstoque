using ErrorOr;
using FluentValidation;

using MediatR;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Handlers.SolicitarEsqueciSenha;

public record SolicitarEsqueciSenhaRequest(string Identificacao) : IRequest<ErrorOr<string?>>;

public class SolicitaresqueciSenhaRequestValidator : AbstractValidator<SolicitarEsqueciSenhaRequest>
{
    public SolicitaresqueciSenhaRequestValidator()
    {
        RuleFor(x => x.Identificacao)
            .NotEmpty()
            .WithMessage("Usuário ou e-mail deve ser preenchido");

        RuleFor(_ => _)
            .Custom((x, validationContext) =>
            {
                if (x.Identificacao.EmailValido())
                {
                    RuleFor(y => y.Identificacao)
                        .EmailAddress()
                        .WithMessage("E-mail invalido");
                }
                else
                {
                    RuleFor(y => y.Identificacao)
                        .MinimumLength(3)
                        .WithMessage("A usuário deve conter no minimo {MinLength} caracteres")
                        .MaximumLength(40)
                        .WithMessage("A usuário deve conter no minimo {MinLength} caracteres");
                }
            });
    }
}