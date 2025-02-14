using ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Models;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Handlers.Login;

public record LoginResponse(string TokenDeAcesso, double ExpiraEm, UsuarioToken UsuarioToken);