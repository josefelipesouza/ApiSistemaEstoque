using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiSistemaEstoque.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrigirItemEstoquePK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ItensEstoque",
                table: "ItensEstoque");

            migrationBuilder.AlterColumn<int>(
                name: "Quantidade",
                table: "ItensEstoque",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Codigo",
                table: "ItensEstoque",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "CodigoEstoque",
                table: "ItensEstoque",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "CodigoItem",
                table: "ItensEstoque",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldDefaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItensEstoque",
                table: "ItensEstoque",
                column: "Codigo");

            migrationBuilder.CreateIndex(
                name: "IX_ItensEstoque_CodigoItem_CodigoEstoque",
                table: "ItensEstoque",
                columns: new[] { "CodigoItem", "CodigoEstoque" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ItensEstoque",
                table: "ItensEstoque");

            migrationBuilder.DropIndex(
                name: "IX_ItensEstoque_CodigoItem_CodigoEstoque",
                table: "ItensEstoque");

            migrationBuilder.AlterColumn<int>(
                name: "Quantidade",
                table: "ItensEstoque",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "CodigoItem",
                table: "ItensEstoque",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "CodigoEstoque",
                table: "ItensEstoque",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "Codigo",
                table: "ItensEstoque",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItensEstoque",
                table: "ItensEstoque",
                columns: new[] { "CodigoItem", "CodigoEstoque" });
        }
    }
}
