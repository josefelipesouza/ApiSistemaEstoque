using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Data;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;
using ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly EstoqueContext _context;

        public UsuarioRepository(EstoqueContext context)
        {
            _context = context;
        }

        public IUnityOfWork UnityOfWork => _context;

        public async Task AdicionarAsync(Usuario usuario, CancellationToken cancellationToken)
        {
            await _context.Usuarios.AddAsync(usuario, cancellationToken);
        }

        public void Atualizar(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
        }

        public async Task<bool> ExisteUsuario(Expression<Func<Usuario, bool>> filtroUsuario, CancellationToken cancellationToken)
        {
            return await _context.Usuarios.AnyAsync(filtroUsuario, cancellationToken);
        }

        public async Task<Usuario?> BuscarPorIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Codigo == id, cancellationToken);
        }

        public Task<Usuario?> BuscarPorIdentityIdAsync(string identityId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<(int? Id, string? CodigoCentroDeCusto, int? IdUsuarioCoordenador, string? Nome, string? Email, string? IdentityId, string? UsuarioWkId)> BuscarDadosCompletosPorIdAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<(int? Id, string? CodigoCentroDeCusto, int? IdUsuarioCoordenador, string? Nome, string? Email, string? IdentityId, string? UsuarioWkId)> BuscarDadosCompletosPorIdAsync(string identityId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<(int Id, string Nome, string Email, string Permissao)>?> BuscarAsync(string nome, string email, string? permissao, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
