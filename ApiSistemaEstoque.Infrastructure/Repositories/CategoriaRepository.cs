using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Data;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;
using ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Repositories;

    public class CategoriaRepository : ICategoriaRepository
    {
        public IUnityOfWork UnitOfWork => _context;

        private readonly EstoqueContext _context;

        public CategoriaRepository(EstoqueContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Categoria categoria, CancellationToken cancellationToken)
        {
            await _context.Categorias.AddAsync(categoria, cancellationToken);
        }

        public async Task<Categoria?> BuscarPorCodigoAsync(int codigo, CancellationToken cancellationToken)
        {
            return await _context.Categorias
                .AsNoTracking()
                .Where(x => x.Codigo == codigo)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Categoria>> ListarAsync(CancellationToken cancellationToken)
        {
            return await _context.Categorias
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public void Atualizar(Categoria categoria)
        {
            _context.Categorias.Update(categoria);
        }

        public void Inativar(Categoria categoria)
        {
            categoria.SetInativar();
            _context.Categorias.Update(categoria);
        }
    }

