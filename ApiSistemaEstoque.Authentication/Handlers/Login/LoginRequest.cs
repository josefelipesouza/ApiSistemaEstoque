using ErrorOr;
using FluentValidation;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Extensions;
using MediatR;


namespace ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Handlers.Login;

public record LoginRequest(string Identificacao, string Senha) : IRequest<ErrorOr<LoginResponse>>;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
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
        
        RuleFor(x => x.Senha)
            .NotEmpty()
            .WithMessage("Senha deve ser preenchida")
            .MinimumLength(6)
            .WithMessage("A senha deve conter no minimo {MinLength} caracteres")
            .WithErrorCode("Authentication.SenhaInvalida");
    }
}