using ApiSistemaEstoque.ApiSistemaEstoque.Application.Interfaces.Data;
using ApiSistemaEstoque.ApiSistemaEstoque.Authentication.EntityConfig;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Entities;
using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Infrastructure.Context
{
    public class EstoqueContext : IdentityDbContext<IdentityUser>, IUnityOfWork
    {
        public EstoqueContext(DbContextOptions<EstoqueContext> options)
            : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Estoque> Estoques { get; set; }
        public DbSet<HistoricoValorItem> HistoricoValorItens { get; set; }
        public DbSet<Item> Itens { get; set; }
        public DbSet<ItemEstoque> ItensEstoque { get; set; }
        public DbSet<ItemMovimentacao> ItensMovimentacao { get; set; }
        public DbSet<KardexDiario> KardexDiario { get; set; }
        public DbSet<Movimentacao> Movimentacoes { get; set; }
        public DbSet<TipoMovimentacao> TiposMovimentacoes { get; set; }
        public DbSet<Unidade> Unidades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleEntityConfig());
            modelBuilder.ApplyConfiguration(new CategoriaEntityTypeConfiguration()); // 👍 Navegação configurada aqui

            // Demais configurações
            modelBuilder.Entity<HistoricoValorItem>().HasKey(h => h.Codigo);
            modelBuilder.Entity<Item>().HasKey(i => i.Codigo);

            modelBuilder.Entity<ItemEstoque>()
                .HasKey(ie => new { ie.CodigoItem, ie.CodigoEstoque });
            modelBuilder.Entity<ItemEstoque>()
                .Property(ie => ie.Quantidade).IsRequired();
            modelBuilder.Entity<ItemEstoque>()
                .HasOne(ie => ie.Item).WithMany().HasForeignKey(ie => ie.CodigoItem).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ItemEstoque>()
                .HasOne(ie => ie.Estoque).WithMany().HasForeignKey(ie => ie.CodigoEstoque).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ItemEstoque>().Property(ie => ie.CodigoItem).HasDefaultValue(0);
            modelBuilder.Entity<ItemEstoque>().Property(ie => ie.CodigoEstoque).HasDefaultValue(0);
            modelBuilder.Entity<ItemEstoque>().Property(ie => ie.Quantidade).HasDefaultValue(0);

            modelBuilder.Entity<ItemMovimentacao>()
                .HasKey(im => new { im.CodigoMovimentacao, im.Item });
            modelBuilder.Entity<ItemMovimentacao>().Property(im => im.CodigoMovimentacao).HasDefaultValue(0);
            modelBuilder.Entity<ItemMovimentacao>().Property(im => im.Item).HasDefaultValue(0);
            modelBuilder.Entity<ItemMovimentacao>().Property(im => im.Quantidade).HasDefaultValue(1);
            modelBuilder.Entity<ItemMovimentacao>()
                .HasOne(im => im.Movimentacao).WithMany(m => m.ItensMovimentacao)
                .HasForeignKey(im => im.CodigoMovimentacao)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<KardexDiario>().HasKey(kd => kd.Codigo);

            modelBuilder.Entity<Movimentacao>().HasKey(m => m.Codigo);
            modelBuilder.Entity<Movimentacao>().Property(m => m.Codigo).ValueGeneratedOnAdd();
            modelBuilder.Entity<Movimentacao>()
                .Property(m => m.Status)
                .HasConversion<string>()
                .HasDefaultValue(StatusMovimentacao.Aguardando);
            modelBuilder.Entity<Movimentacao>()
                .Property(m => m.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<Movimentacao>()
                .Property(m => m.updated_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<Movimentacao>()
                .HasOne(m => m.TipoMovimentacao).WithMany()
                .HasForeignKey(m => m.CodigoTipoMovimentacao)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Movimentacao>()
                .HasMany(m => m.ItensMovimentacao).WithOne(im => im.Movimentacao)
                .HasForeignKey(im => im.CodigoMovimentacao)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TipoMovimentacao>().HasKey(tm => tm.Codigo);
            modelBuilder.Entity<Unidade>().HasKey(u => u.Codigo);
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await SaveChangesAsync(cancellationToken);
        }
    }
}
