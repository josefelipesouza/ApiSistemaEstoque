namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Data;

public interface IUnityOfWork 
{
    Task CommitAsync(CancellationToken cancellationToken);
}