using ApiSistemaEstoque.ApiSistemaEstoque.Authentication.EntityConfig;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Authentication.Context;

    public class AuthContext : IdentityDbContext<IdentityUser>
    {
        public AuthContext(DbContextOptions<AuthContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Aplique as configurações específicas das entidades do sistema de estoque
            //modelBuilder.ApplyConfiguration(new ProdutoEntityConfig()); // Exemplo de configuração de entidade
            //modelBuilder.ApplyConfiguration(new CategoriaEntityConfig()); // Exemplo de configuração de entidade
            modelBuilder.ApplyConfiguration(new RoleEntityConfig());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            optionsBuilder.LogTo(x => Console.WriteLine(x)).EnableSensitiveDataLogging();
#endif
            base.OnConfiguring(optionsBuilder);
        }
    }
