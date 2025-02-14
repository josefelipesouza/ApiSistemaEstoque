using ErrorOr;
using FluentValidation;
using MediatR;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Handlers.AlterarSenha;

public record AlterarSenhaRequest(string Usuario, string Token, string Senha) : IRequest<ErrorOr<string?>>;

public class AlterarSenhaRequestValidator : AbstractValidator<AlterarSenhaRequest>
{
    public AlterarSenhaRequestValidator()
    {
        RuleFor(x => x.Senha)
            .NotEmpty()
            .WithMessage("A senha é obrigatória")
            .Length(6, 20)
            .WithMessage("Senha inválida");

        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("Token inválido.");
        
        RuleFor(x => x.Usuario)
            .NotEmpty()
            .WithMessage("Usuário deve ser preenchido")
            .MinimumLength(3)
            .WithMessage("A usuário deve conter no minimo {MinLength} caracteres")
            .MaximumLength(40)
            .WithMessage("A usuário deve conter no minimo {MinLength} caracteres");
    }
}