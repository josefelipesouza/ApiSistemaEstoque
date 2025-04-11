using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Data;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;
using ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Repositories;

public class UnidadeRepository : IUnidadeRepository
{
    public IUnityOfWork UnitOfWork => _context;

    private readonly EstoqueContext _context;

    public UnidadeRepository(EstoqueContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Unidade unidade, CancellationToken cancellationToken)
    {
        await _context.Unidades.AddAsync(unidade, cancellationToken);
    }

    public async Task<Unidade?> BuscarPorCodigoAsync(int codigo, CancellationToken cancellationToken)
    {
        return await _context.Unidades
            .AsNoTracking()
            .Where(x => x.Codigo == codigo)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Unidade>> ListarAsync(CancellationToken cancellationToken)
    {
        return await _context.Unidades
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public void Atualizar(Unidade unidade)
    {
        _context.Unidades.Update(unidade);
    }

    public void Inativar(Unidade unidade)
    {
        unidade.SetInativar();
        _context.Unidades.Update(unidade);
    }
}
