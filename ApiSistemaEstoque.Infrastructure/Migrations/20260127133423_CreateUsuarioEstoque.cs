using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiSistemaEstoque.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateUsuarioEstoque : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsuariosEstoques",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CodigoUsuario = table.Column<string>(type: "TEXT", nullable: false),
                    CodigoEstoque = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosEstoques", x => x.Codigo);
                    table.ForeignKey(
                        name: "FK_UsuariosEstoques_Estoques_CodigoEstoque",
                        column: x => x.CodigoEstoque,
                        principalTable: "Estoques",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosEstoques_CodigoEstoque",
                table: "UsuariosEstoques",
                column: "CodigoEstoque");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosEstoques_CodigoUsuario_CodigoEstoque",
                table: "UsuariosEstoques",
                columns: new[] { "CodigoUsuario", "CodigoEstoque" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuariosEstoques");
        }
    }
}
