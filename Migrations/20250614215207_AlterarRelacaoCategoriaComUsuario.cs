using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiSistemaEstoque.Migrations
{
    /// <inheritdoc />
    public partial class AlterarRelacaoCategoriaComUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Categorias_UsuarioCadastro",
                table: "Categorias",
                column: "UsuarioCadastro");

            migrationBuilder.AddForeignKey(
                name: "FK_Categorias_AspNetUsers_UsuarioCadastro",
                table: "Categorias",
                column: "UsuarioCadastro",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categorias_AspNetUsers_UsuarioCadastro",
                table: "Categorias");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_UsuarioCadastro",
                table: "Categorias");
        }
    }
}
