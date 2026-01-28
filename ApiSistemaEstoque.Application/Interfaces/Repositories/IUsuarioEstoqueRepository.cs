namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;

public interface IUsuarioEstoqueRepository
{
    Task<bool> ExisteVinculoAsync(
        string codigoUsuario,
        int codigoEstoque,
        CancellationToken cancellationToken);

    Task<int?> ObterCodigoEstoquePorUsuarioAsync(
        string codigoUsuario,
        CancellationToken cancellationToken);    
}
