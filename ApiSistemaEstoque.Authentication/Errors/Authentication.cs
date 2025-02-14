using ErrorOr;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Errors;

public static partial class Authentication
{
    public static Error UsuarioBloqueadoTentativasInvalidas = Error.Validation(
        code: "Authentication.UsuarioBloqueadoTentativasInvalidas",
        description: "Usuario temporariamente bloqueado por tentativas inválidas, tente novamente mais tarde");

    public static Error UsuarioSenhaIncorretas = Error.Unauthorized(
        code: "Authentication.UsuarioSenhaIncorretas",
        description: "Usuario ou senha incorretos.");

    public static Error UsuarioNaoPossuiPermissao = Error.Unauthorized(
        code: "Authentication.UsuarioNaoPossuiPermissao",
        description: "Usuario não possui permissão para acessar a aplicação.");

    public static Error UsuarioNaoEncontrado = Error.NotFound(
        code: "Authentication.UsuarioNaoEncontrado",
        description: "Dados do usuário inválidos");
    
    public static Error CoordenadorNaoEncontrado = Error.NotFound(
        code: "Authentication.CoordenadorNaoEncontrado",
        description: "Dados do coordenador inválidos");

    public static Error UsuarioDuplicado = Error.Validation(
        code: "Authentication.UsuarioDuplicado",
        description: "Dados do usuário inválidos");

    public static Error UsuarioDadosImcompletos = Error.Validation(
        code: "Authentication.UsuarioDadosImcompletos",
        description: "Dados cadastrais imcompletos, verifique o cadastro do usuário no WK"); 
    
    public static Error PermissaoNaoEncontrada = Error.Validation(
        code: "Authentication.PermissaoNaoEncontrada",
        description: "Permissoes invalidas");
}