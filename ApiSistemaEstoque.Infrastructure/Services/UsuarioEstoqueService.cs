using ApiSistemaEstoque.Application.Interfaces.UsuariosEstoque;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;
using ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Context;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Services;

public class UsuarioEstoqueService : IUsuarioEstoqueService
{
    private readonly EstoqueContext _context;

    public UsuarioEstoqueService(EstoqueContext context)
    {
        _context = context;
    }

    public async Task VincularAsync(
        string codigoUsuario,
        int codigoEstoque,
        CancellationToken cancellationToken)
    {
        var usuarioEstoque = new UsuarioEstoque(
            codigoUsuario,
            codigoEstoque
        );

        _context.UsuariosEstoques.Add(usuarioEstoque);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
