using ApiSistemaEstoque.ApiSistemaEstoque.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiSistemaEstoque.ApiSistemaEstoque.Authentication.EntityConfig;

    public class RoleEntityConfig : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "1a2b3c4d-5e6f-7g8h-9i0j-k1l2m3n4o5p6",
                    Name = nameof(PerfilDeAcessoEstoque.GerenteDeEstoque),
                    NormalizedName = "GERENTEDEESTOQUE"
                },
                new IdentityRole
                {
                    Id = "2b3c4d5e-6f7g-8h9i-0j1k-l2m3n4o5p6q",
                    Name = nameof(PerfilDeAcessoEstoque.Almoxarife),
                    NormalizedName = "ALMOXARIFE"
                },
                new IdentityRole
                {
                    Id = "3c4d5e6f-7g8h-9i0j-k1l2-m3n4o5p6q7r",
                    Name = nameof(PerfilDeAcessoEstoque.GestorDeCompras),
                    NormalizedName = "GESTORDECOMPRAS"
                },
                new IdentityRole
                {
                    Id = "4d5e6f7g-8h9i-0j1k-l2m3-n4o5p6q7r8s",
                    Name = nameof(PerfilDeAcessoEstoque.GestorDeVendas),
                    NormalizedName = "GESTORDEVENDAS"
                },
                new IdentityRole
                {
                    Id = "5e6f7g8h-9i0j-k1l2-m3n4-o5p6q7r8s9t",
                    Name = nameof(PerfilDeAcessoEstoque.GestorDeInventario),
                    NormalizedName = "GESTORDEINVENTARIO"
                },
                new IdentityRole
                {
                    Id = "6f7g8h9i-0j1k-l2m3-n4o5-p6q7r8s9t0u",
                    Name = nameof(PerfilDeAcessoEstoque.GerenteDeLogistica),
                    NormalizedName = "GERENTEDELOGISTICA"
                }
            );
        }
    }
