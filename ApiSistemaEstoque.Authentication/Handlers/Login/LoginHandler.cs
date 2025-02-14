using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Models;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;


namespace ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Handlers.Login;

public class LoginHandler(IMediator mediator,
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager,
        IUsuarioRepository usuarioRepository,
        IOptions<AppSettings> appSettings)
    : BaseHandler(mediator),
        IRequestHandler<LoginRequest, ErrorOr<LoginResponse>>
{
    private readonly AppSettings _appSettings = appSettings.Value;

    public async Task<ErrorOr<LoginResponse>> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        if (Validar(request, new LoginRequestValidator()) is var resultado && resultado.Any())
            return resultado;

        var user = await ValidaDadosUsuario(request.Identificacao, cancellationToken);

        if (user.IsError)
            return user.Errors;

        var result = await signInManager.PasswordSignInAsync(user.Value.UserName!, request.Senha, false, true);

        if (result.Succeeded)
            return await GerarJwt(user.Value.UserName!);

        return result.IsLockedOut
            ? Errors.Authentication.UsuarioBloqueadoTentativasInvalidas
            : Errors.Authentication.UsuarioSenhaIncorretas;
    }

    private async Task<ErrorOr<IdentityUser>> ValidaDadosUsuario(string identificacao,
        CancellationToken cancellationToken)
    {
        IdentityUser? user;
        if (identificacao.EmailValido())
        {
            user = await userManager.FindByEmailAsync(identificacao);
        }
        else
        {
            user = await userManager.FindByNameAsync(identificacao);
        }
        
        if (user is null)
            return Errors.Authentication.UsuarioNaoEncontrado;

        var usuarioAplicacao = await usuarioRepository.BuscarPorIdentityIdAsync(user.Id, cancellationToken);

        if (usuarioAplicacao is null)
            return Errors.Authentication.UsuarioNaoEncontrado;

        // var usuarioWk = await moduloEmpresarialRepository.BuscarRequisitante(usuarioAplicacao.UsuarioWkId, cancellationToken);
        //
        // if (usuarioWk is null)
        //     return Errors.Authentication.UsuarioNaoEncontrado;
        //
        // if (!usuarioWk.Ativo)
        //     return Errors.Authentication.UsuarioNaoPossuiPermissao;

        return user;
    }

    private async Task<LoginResponse> GerarJwt(string nomeUsuario)
    {
        var user = await userManager.FindByNameAsync(nomeUsuario);
        var claims = await userManager.GetClaimsAsync(user!);
        var identityClaims = await ObterClaimsUsuario(user!, claims);
        var encodedToken = CodificarToken(identityClaims);
        return ObterRespostaToken(user!, claims, encodedToken);
    }

    private LoginResponse ObterRespostaToken(IdentityUser user, IEnumerable<Claim> claims, string encodedToken)
    {
        return new LoginResponse(encodedToken, TimeSpan.FromHours(_appSettings.ExpiracaoEmHoras).TotalSeconds,
            new UsuarioToken(user.Id, user.UserName!, user.Email!,
                claims.Select(x => new UsuarioClaim(x.Value, x.Type))));
    }

    private string CodificarToken(ClaimsIdentity identityClaims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Segredo);
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = _appSettings.Emissor,
            Audience = _appSettings.ValidoEm,
            Subject = identityClaims,
            Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoEmHoras),
            SigningCredentials = new SigningCredentials
            (
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256
            )
        });

        var encodedToken = tokenHandler.WriteToken(token);
        return encodedToken;
    }

    private async Task<ClaimsIdentity> ObterClaimsUsuario(IdentityUser user, ICollection<Claim> claims)
    {
        var userRoles = await userManager.GetRolesAsync(user);
        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName!));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email!));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpocDate(DateTime.UtcNow).ToString())); // quando expira
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpocDate(DateTime.UtcNow).ToString(),
            ClaimValueTypes.Integer64)); // quando expira

        foreach (var role in userRoles)
            claims.Add(new Claim("role", role));

        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);
        return identityClaims;
    }

    private static long ToUnixEpocDate(DateTime date)
        => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
            .TotalSeconds);
}