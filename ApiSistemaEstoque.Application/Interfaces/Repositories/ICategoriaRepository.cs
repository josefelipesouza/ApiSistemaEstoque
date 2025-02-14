using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Data;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;

    public interface ICategoriaRepository
    {
        IUnitOfWork UnitOfWork { get; }

        Task AdicionarAsync(Categoria categoria, CancellationToken cancellationToken);

        Task<Categoria?> BuscarPorCodigoAsync(int codigo, CancellationToken cancellationToken);

        Task<IEnumerable<Categoria>> ListarAsync(CancellationToken cancellationToken);

        void Atualizar(Categoria categoria);

        void Inativar(Categoria categoria);
    }

