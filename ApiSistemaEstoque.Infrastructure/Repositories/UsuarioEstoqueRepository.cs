using ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Context;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Repositories;

public class UsuarioEstoqueRepository : IUsuarioEstoqueRepository
{
    private readonly EstoqueContext _context;

    public UsuarioEstoqueRepository(EstoqueContext context)
    {
        _context = context;
    }

    public async Task<bool> ExisteVinculoAsync(
        string codigoUsuario,
        int codigoEstoque,
        CancellationToken cancellationToken)
    {
        return await _context.UsuariosEstoques.AnyAsync(
            ue =>
                ue.CodigoUsuario == codigoUsuario &&
                ue.CodigoEstoque == codigoEstoque,
            cancellationToken
        );
    }
}
