// Application/Handlers/Auth/Registrar/RegistrarUsuarioResponse.cs
namespace ApiSistemaEstoque.Application.Handlers.Usuario.Auth.Registrar;

public class RegistrarUsuarioResponse
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }

    public RegistrarUsuarioResponse(string id, string email, string role)
    {
        Id = id;
        Email = email;
        Role = role;
    }
}
