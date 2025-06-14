using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiSistemaEstoque.Migrations
{
    /// <inheritdoc />
    public partial class AlterarUsuarioCadastroParaString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Itens_Usuario_UsuarioCadastro",
                table: "Itens");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Setor_CodigoSetor",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Itens_UsuarioCadastro",
                table: "Itens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario");

            migrationBuilder.RenameTable(
                name: "Usuario",
                newName: "Usuarios");

            migrationBuilder.RenameIndex(
                name: "IX_Usuario_CodigoSetor",
                table: "Usuarios",
                newName: "IX_Usuarios_CodigoSetor");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioCadastro",
                table: "Unidades",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioCadastro",
                table: "TiposMovimentacoes",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioCadastro",
                table: "Setor",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioCadastro",
                table: "Itens",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioCadastro",
                table: "Estoques",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioCadastro",
                table: "Categorias",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "Inativo",
                table: "Categorias",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioCadastro",
                table: "Usuarios",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuarios",
                table: "Usuarios",
                column: "Codigo");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Setor_CodigoSetor",
                table: "Usuarios",
                column: "CodigoSetor",
                principalTable: "Setor",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Setor_CodigoSetor",
                table: "Usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuarios",
                table: "Usuarios");

            migrationBuilder.RenameTable(
                name: "Usuarios",
                newName: "Usuario");

            migrationBuilder.RenameIndex(
                name: "IX_Usuarios_CodigoSetor",
                table: "Usuario",
                newName: "IX_Usuario_CodigoSetor");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioCadastro",
                table: "Unidades",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioCadastro",
                table: "TiposMovimentacoes",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioCadastro",
                table: "Setor",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioCadastro",
                table: "Itens",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioCadastro",
                table: "Estoques",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioCadastro",
                table: "Categorias",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Inativo",
                table: "Categorias",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioCadastro",
                table: "Usuario",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario",
                column: "Codigo");

            migrationBuilder.CreateIndex(
                name: "IX_Itens_UsuarioCadastro",
                table: "Itens",
                column: "UsuarioCadastro");

            migrationBuilder.AddForeignKey(
                name: "FK_Itens_Usuario_UsuarioCadastro",
                table: "Itens",
                column: "UsuarioCadastro",
                principalTable: "Usuario",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Setor_CodigoSetor",
                table: "Usuario",
                column: "CodigoSetor",
                principalTable: "Setor",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
