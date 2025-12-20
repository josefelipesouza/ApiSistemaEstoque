using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiSistemaEstoque.Migrations
{
    /// <inheritdoc />
    public partial class AlteraTipoDoCampoCodigoUsuarioEstoque : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "TiposMovimentacoes",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Movimentacoes",
                type: "TEXT",
                nullable: false,
                defaultValue: "Novo",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "Aguardando");

            migrationBuilder.AlterColumn<string>(
                name: "CodigoUsuarioEstoqueSolicitante",
                table: "Movimentacoes",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "CodigoUsuarioEstoqueSolicitado",
                table: "Movimentacoes",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "TiposMovimentacoes",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Movimentacoes",
                type: "TEXT",
                nullable: false,
                defaultValue: "Aguardando",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "Novo");

            migrationBuilder.AlterColumn<int>(
                name: "CodigoUsuarioEstoqueSolicitante",
                table: "Movimentacoes",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "CodigoUsuarioEstoqueSolicitado",
                table: "Movimentacoes",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
