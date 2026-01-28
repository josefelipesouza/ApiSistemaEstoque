using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiSistemaEstoque.Migrations
{
    /// <inheritdoc />
    public partial class AjusteItemMovimentacaoCodigoPK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ItensMovimentacao",
                table: "ItensMovimentacao");

            migrationBuilder.AlterColumn<int>(
                name: "Codigo",
                table: "ItensMovimentacao",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItensMovimentacao",
                table: "ItensMovimentacao",
                column: "Codigo");

            migrationBuilder.CreateIndex(
                name: "IX_ItensMovimentacao_CodigoMovimentacao",
                table: "ItensMovimentacao",
                column: "CodigoMovimentacao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ItensMovimentacao",
                table: "ItensMovimentacao");

            migrationBuilder.DropIndex(
                name: "IX_ItensMovimentacao_CodigoMovimentacao",
                table: "ItensMovimentacao");

            migrationBuilder.AlterColumn<int>(
                name: "Codigo",
                table: "ItensMovimentacao",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItensMovimentacao",
                table: "ItensMovimentacao",
                columns: new[] { "CodigoMovimentacao", "Item" });
        }
    }
}
