using ErrorOr;
using FluentValidation;
using MediatR;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Handlers.Registrar;

public record RegistrarRequest(
    string UsuarioWkId,
    string CodigoCentroDeCusto,
    int IdUsuarioCoordenador,
    string Nome,
    IEnumerable<string> Roles) : IRequest<ErrorOr<bool>>;

public class RegistrarRequestValidator : AbstractValidator<RegistrarRequest>
{
    public RegistrarRequestValidator()
    {
        RuleFor(x => x.UsuarioWkId)
            .NotEmpty()
            .WithMessage("Usuário WK deve ser preenchido");

        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Nome deve ser preenchido")
            .Length(1, 100)
            .WithMessage("O nome deve conter entre {MinLength} e {MaxLength} caracteres");

        RuleFor(x => x.Roles)
            .NotEmpty()
            .WithMessage("Permissão deve ser preenchida");

        RuleFor(x => x.CodigoCentroDeCusto)
            .NotEmpty()
            .WithMessage("O centro de custo deve ser informado")
            .Length(1, 20)
            .WithMessage("O codigo do centro de custo deve conter entre {MinLength} e {MaxLength} caracteres");

        
            RuleFor(x => x.IdUsuarioCoordenador)
                .NotEmpty()
                .WithMessage("Coordenador deve ser preenchido")
                .GreaterThan(0)
                .WithMessage("Coordenador invalido");
    }
}