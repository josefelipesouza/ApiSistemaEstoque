namespace ApiSistemaEstoque.Application.Interfaces.UsuariosEstoque;

public interface IUsuarioEstoqueService
{
    Task VincularAsync(
        string codigoUsuario,
        int codigoEstoque,
        CancellationToken cancellationToken);
}
