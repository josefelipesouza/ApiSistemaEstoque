using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Data;
using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Repositories;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;
using ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Repositories;

public class ItemRepository : IItemRepository
{
    public IUnityOfWork UnitOfWork => _context;

    private readonly EstoqueContext _context;

    public ItemRepository(EstoqueContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Item item, CancellationToken cancellationToken)
    {
        await _context.Itens.AddAsync(item, cancellationToken);
    }

    public async Task<Item?> BuscarPorCodigoAsync(int codigo, CancellationToken cancellationToken)
    {
        return await _context.Itens
            .Include(i => i.Categoria)      // Inclui a relação com Categoria
            .Include(i => i.Unidade)        // Inclui a relação com Unidade
            .AsNoTracking()
           .Where(x => x.Codigo == codigo && x.Inativo == Status.Ativo)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Item>> ListarAsync(CancellationToken cancellationToken)
    {
        return await _context.Itens
            .Include(i => i.Categoria)      // Inclui a relação com Categoria
            .Include(i => i.Unidade)        // Inclui a relação com Unidade
            .AsNoTracking()
            .Where(x => x.Inativo == Status.Ativo)
            .ToListAsync(cancellationToken);
    }

    public void Atualizar(Item item)
    {
        _context.Itens.Update(item);
    }

    public void Inativar(Item item)
    {
        item.SetInativar();
        _context.Itens.Update(item);
    }
}
