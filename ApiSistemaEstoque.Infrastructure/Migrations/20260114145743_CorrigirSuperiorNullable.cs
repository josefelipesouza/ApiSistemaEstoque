using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiSistemaEstoque.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrigirSuperiorNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Superior",
                table: "Categorias",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Superior",
                table: "Categorias",
                column: "Superior");

            migrationBuilder.AddForeignKey(
                name: "FK_Categorias_Categorias_Superior",
                table: "Categorias",
                column: "Superior",
                principalTable: "Categorias",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categorias_Categorias_Superior",
                table: "Categorias");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_Superior",
                table: "Categorias");

            migrationBuilder.AlterColumn<int>(
                name: "Superior",
                table: "Categorias",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }
    }
}
