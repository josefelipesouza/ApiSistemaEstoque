using System.Text;
using ErrorOr;

using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Handlers.SolicitarEsqueciSenha;

public class SolicitarEsqueciSenhaHandler : BaseHandler, IRequestHandler<SolicitarEsqueciSenhaRequest, ErrorOr<string?>>
{
    private readonly IEmailService _emailService;

    private readonly IUsuarioRepository _usuarioRepository;
    private readonly UserManager<IdentityUser> _userManager;

    public SolicitarEsqueciSenhaHandler(
        IMediator mediator,
        UserManager<IdentityUser> userManager,
        IUsuarioRepository usuarioRepository,
        IEmailService emailService,

    {
        _userManager = userManager;
        _usuarioRepository = usuarioRepository;
        _emailService = emailService;

    }

    public async Task<ErrorOr<string?>> Handle(SolicitarEsqueciSenhaRequest request,
        CancellationToken cancellationToken)
    {
        if (Validar(request, new SolicitaresqueciSenhaRequestValidator()) is var resultado && resultado.Any())
            return resultado;
        
        IdentityUser? identityUser;
        if (request.Identificacao.EmailValido())
        {
            identityUser = await _userManager.FindByEmailAsync(request.Identificacao);
        }
        else
        {
            identityUser = await _userManager.FindByNameAsync(request.Identificacao);
        }
        
        if (identityUser is null)
            return Errors.Authentication.UsuarioNaoEncontrado;

        var tokenEsqueciSenha = await _userManager.GeneratePasswordResetTokenAsync(identityUser);

        var corpoEmail = FormartaCorpoEmail(identityUser.UserName, tokenEsqueciSenha, identityUser.UserName, urlBase);


            "Solicitação de troca de senha", corpoEmail, cancellationToken);

        return string.Empty;
    }

    private string FormartaCorpoEmail(string? nomeUsuario, string? tokenResetarSenha, string? usuarioLogin,
        string? urlBase)
    {
        var emailBuilder = new StringBuilder();
        emailBuilder.Append("<!DOCTYPE html>\n<html lang=\"en\">\n<head>\n");
        emailBuilder.Append("    <meta charset=\"UTF-8\">\n");
        emailBuilder.Append("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n");
        emailBuilder.Append("    <style>\n");
        emailBuilder.Append("        /* Seu estilo CSS aqui */\n");
        emailBuilder.Append("    </style>\n");
        emailBuilder.Append("</head>\n<body>\n");
        emailBuilder.Append("    <div class=\"container\">\n");
        emailBuilder.Append($"        <h1>Redefinição de Senha</h1>\n");
        emailBuilder.Append($"        <p>Olá, {nomeUsuario}</p>\n");
        emailBuilder.Append(
            "        <p>Recebemos uma solicitação para redefinir a senha da sua conta. Se foi você, clique no botão abaixo para prosseguir:</p>\n");
        emailBuilder.Append(
            $"        <a class=\"btn\" style=\"display: inline-block;padding: 10px 20px;background-color: #dd99ab;color: #ffffff;text-decoration: none;border-radius: 5px;\" href=\"{urlBase}?token={tokenResetarSenha}&usuario={usuarioLogin}\">Redefinir Senha</a>\n");
        emailBuilder.Append("        <p>Se você não solicitou a troca de senha, pode ignorar este e-mail.</p>\n");
        emailBuilder.Append("        <p>Obrigado,<br>Equipe de Suporte</p>\n");
        emailBuilder.Append("    </div>\n</body>\n</html>");

        return emailBuilder.ToString();
    }
}