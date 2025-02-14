using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Handlers.AlterarSenha;

public class AlterarSenhaHandler : BaseHandler, IRequestHandler<AlterarSenhaRequest, ErrorOr<string?>>
{
    private readonly UserManager<IdentityUser> _userManager;
    
    public AlterarSenhaHandler(
        IMediator mediator, 
        UserManager<IdentityUser> userManager) : base(mediator)
    {
        _userManager = userManager;
    }

    public async Task<ErrorOr<string?>> Handle(AlterarSenhaRequest request, CancellationToken cancellationToken)
    {
        if (Validar(request, new AlterarSenhaRequestValidator()) is var resultado && resultado.Any())
            return resultado;

        var identityUser = await _userManager.FindByNameAsync(request.Usuario);

        if (identityUser is null)
            return Errors.Authentication.UsuarioNaoEncontrado;

        var resultadoTrocaDeSenha = await _userManager.ResetPasswordAsync(identityUser, request.Token, request.Senha);

        if (!resultadoTrocaDeSenha.Succeeded)
        {
            return resultadoTrocaDeSenha.Errors
                .Select(erro => Error.Validation(code: erro.Code, description: erro.Description))
                .ToList();
        }

        return string.Empty;
    }
}