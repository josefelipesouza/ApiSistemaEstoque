using ErrorOr;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Handlers;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Services;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Extensions;
using ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;



namespace ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Handlers.Registrar;

public class RegistrarHandler : BaseHandler, IRequestHandler<RegistrarRequest, ErrorOr<bool>>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPermissaoService _permissaoService;

    public RegistrarHandler(
        IMediator mediator,
        UserManager<IdentityUser> userManager,
        IUsuarioRepository usuarioRepository,
        IPermissaoService permissaoService) : base(mediator)
    {
        _userManager = userManager;
        _usuarioRepository = usuarioRepository;
        _permissaoService = permissaoService;
    }

    public async Task<ErrorOr<bool>> Handle(RegistrarRequest request, CancellationToken cancellationToken)
    {
        if (Validar(request, new RegistrarRequestValidator()) is var resultado && resultado.Any())
            return resultado;

        var usuarioWk = await _moduloEmpresarialRepository.BuscarRequisitante(request.UsuarioWkId, cancellationToken);
        if (usuarioWk is null)
            return Errors.Authentication.Wk.UsuarioNaoEncontrado;

        if (!usuarioWk.Email.EmailValido())
            return Errors.Authentication.Wk.UsuarioEmailInvalido;

        var existeUsuario =
            _usuarioRepository.ExisteUsuario((usuario) => usuario.UsuarioWkId == usuarioWk.Id, cancellationToken);

        if (await existeUsuario)
            return Errors.Authentication.UsuarioDuplicado;

        var contaEmpresarial =
            await _empresarialRepository.BuscarContaEmpresarial(request.CodigoCentroDeCusto, cancellationToken);

        if (contaEmpresarial.IsError)
            return contaEmpresarial.Errors;

            var coordenador =
                await _usuarioRepository.BuscarPorIdAsync(request.IdUsuarioCoordenador, cancellationToken);
            if (coordenador is null)
                return Errors.Authentication.CoordenadorNaoEncontrado;

        var permissaoResponse = _permissaoService.FormataPermissoesAplicacao();

        foreach (var role in request.Roles)
        {
            if (permissaoResponse.PerfisDeAcesso.Any(x => x.Valor == role))
            {
                continue;
            }

            return Errors.Authentication.PermissaoNaoEncontrada;
        }

        var user = new IdentityUser()
        {
            UserName = usuarioWk.Email,
            Email = usuarioWk.Email
        };

        var result = await _userManager.CreateAsync(user, "123@Mudar");

        if (!result.Succeeded)
        {
            return result.Errors
                .Select(erro => Error.Validation(code: erro.Code, description: erro.Description))
                .ToList();
        }

        _ = await _userManager.AddToRolesAsync(user, request.Roles);

        var usuario = new Usuario(user.Id, usuarioWk.Id, request.Nome)
        {
            IdUsuarioCoordenador = request.IdUsuarioCoordenador,
            CodigoCentroDeCusto = contaEmpresarial.Value.Codigo
        };

        await _usuarioRepository.AdicionarAsync(usuario, CancellationToken.None);
        await _usuarioRepository.UnityOfWork.Commit();

        return true;
    }
    
}