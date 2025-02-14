using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiSistemaEstoque.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Descricao = table.Column<string>(type: "TEXT", nullable: false),
                    Superior = table.Column<int>(type: "INTEGER", nullable: false),
                    UsuarioCadastro = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Inativo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Codigo);
                });

            migrationBuilder.CreateTable(
                name: "Estoques",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Descricao = table.Column<string>(type: "TEXT", nullable: false),
                    UsuarioCadastro = table.Column<int>(type: "INTEGER", nullable: false),
                    Localizacao = table.Column<string>(type: "TEXT", nullable: false),
                    Responsavel = table.Column<int>(type: "INTEGER", nullable: false),
                    Superior = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Inativo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estoques", x => x.Codigo);
                });

            migrationBuilder.CreateTable(
                name: "HistoricoValorItens",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CodigoProduto = table.Column<int>(type: "INTEGER", nullable: true),
                    DataInicial = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataFinal = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Valor = table.Column<float>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoValorItens", x => x.Codigo);
                });

            migrationBuilder.CreateTable(
                name: "KardexDiario",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Data = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CodigoMovimentacao = table.Column<int>(type: "INTEGER", nullable: true),
                    CodigoItem = table.Column<int>(type: "INTEGER", nullable: true),
                    Quantidade = table.Column<int>(type: "INTEGER", nullable: true),
                    CodigoEstoqueOrigem = table.Column<int>(type: "INTEGER", nullable: true),
                    QuantidadeEstoqueOrigemInicial = table.Column<int>(type: "INTEGER", nullable: true),
                    QuantidadeEstoqueOrigemFinal = table.Column<int>(type: "INTEGER", nullable: true),
                    CodigoEstoqueDestino = table.Column<int>(type: "INTEGER", nullable: true),
                    QuantidadeEstoqueDestinoInicial = table.Column<int>(type: "INTEGER", nullable: true),
                    QuantidadeEstoqueDestinoFinal = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KardexDiario", x => x.Codigo);
                });

            migrationBuilder.CreateTable(
                name: "Setor",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    UsuarioCadastro = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Inativo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setor", x => x.Codigo);
                });

            migrationBuilder.CreateTable(
                name: "TiposMovimentacoes",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tipo = table.Column<string>(type: "TEXT", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true),
                    UsuarioCadastro = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposMovimentacoes", x => x.Codigo);
                });

            migrationBuilder.CreateTable(
                name: "Unidades",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Descricao = table.Column<string>(type: "TEXT", nullable: false),
                    UsuarioCadastro = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Inativo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unidades", x => x.Codigo);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    CodigoSetor = table.Column<int>(type: "INTEGER", nullable: false),
                    UsuarioCadastro = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Inativo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Codigo);
                    table.ForeignKey(
                        name: "FK_Usuario_Setor_CodigoSetor",
                        column: x => x.CodigoSetor,
                        principalTable: "Setor",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Movimentacoes",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Status = table.Column<string>(type: "TEXT", nullable: true, defaultValue: "Aguardando"),
                    CodigoTipoMovimentacao = table.Column<int>(type: "INTEGER", nullable: true),
                    CodigoEstoqueSolicitante = table.Column<int>(type: "INTEGER", nullable: true),
                    CodigoUsuarioEstoqueSolicitante = table.Column<int>(type: "INTEGER", nullable: true),
                    CodigoEstoqueSolicitado = table.Column<int>(type: "INTEGER", nullable: true),
                    CodigoUsuarioEstoqueSolicitado = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimentacoes", x => x.Codigo);
                    table.ForeignKey(
                        name: "FK_Movimentacoes_TiposMovimentacoes_CodigoTipoMovimentacao",
                        column: x => x.CodigoTipoMovimentacao,
                        principalTable: "TiposMovimentacoes",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Itens",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Descricao = table.Column<string>(type: "TEXT", nullable: false),
                    QuantidadeMinima = table.Column<int>(type: "INTEGER", nullable: false),
                    Referencia = table.Column<string>(type: "TEXT", nullable: false),
                    CodigoCategoria = table.Column<int>(type: "INTEGER", nullable: false),
                    CodigoUnidade = table.Column<int>(type: "INTEGER", nullable: false),
                    UsuarioCadastro = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Inativo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itens", x => x.Codigo);
                    table.ForeignKey(
                        name: "FK_Itens_Categorias_CodigoCategoria",
                        column: x => x.CodigoCategoria,
                        principalTable: "Categorias",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Itens_Unidades_CodigoUnidade",
                        column: x => x.CodigoUnidade,
                        principalTable: "Unidades",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Itens_Usuario_UsuarioCadastro",
                        column: x => x.UsuarioCadastro,
                        principalTable: "Usuario",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItensMovimentacao",
                columns: table => new
                {
                    CodigoMovimentacao = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    Item = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    Codigo = table.Column<int>(type: "INTEGER", nullable: true),
                    Quantidade = table.Column<int>(type: "INTEGER", nullable: true, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensMovimentacao", x => new { x.CodigoMovimentacao, x.Item });
                    table.ForeignKey(
                        name: "FK_ItensMovimentacao_Movimentacoes_CodigoMovimentacao",
                        column: x => x.CodigoMovimentacao,
                        principalTable: "Movimentacoes",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItensEstoque",
                columns: table => new
                {
                    CodigoItem = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    CodigoEstoque = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    Codigo = table.Column<int>(type: "INTEGER", nullable: true),
                    Quantidade = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EstoqueCodigo = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensEstoque", x => new { x.CodigoItem, x.CodigoEstoque });
                    table.ForeignKey(
                        name: "FK_ItensEstoque_Estoques_CodigoEstoque",
                        column: x => x.CodigoEstoque,
                        principalTable: "Estoques",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItensEstoque_Estoques_EstoqueCodigo",
                        column: x => x.EstoqueCodigo,
                        principalTable: "Estoques",
                        principalColumn: "Codigo");
                    table.ForeignKey(
                        name: "FK_ItensEstoque_Itens_CodigoItem",
                        column: x => x.CodigoItem,
                        principalTable: "Itens",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Itens_CodigoCategoria",
                table: "Itens",
                column: "CodigoCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Itens_CodigoUnidade",
                table: "Itens",
                column: "CodigoUnidade");

            migrationBuilder.CreateIndex(
                name: "IX_Itens_UsuarioCadastro",
                table: "Itens",
                column: "UsuarioCadastro");

            migrationBuilder.CreateIndex(
                name: "IX_ItensEstoque_CodigoEstoque",
                table: "ItensEstoque",
                column: "CodigoEstoque");

            migrationBuilder.CreateIndex(
                name: "IX_ItensEstoque_EstoqueCodigo",
                table: "ItensEstoque",
                column: "EstoqueCodigo");

            migrationBuilder.CreateIndex(
                name: "IX_Movimentacoes_CodigoTipoMovimentacao",
                table: "Movimentacoes",
                column: "CodigoTipoMovimentacao");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_CodigoSetor",
                table: "Usuario",
                column: "CodigoSetor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoricoValorItens");

            migrationBuilder.DropTable(
                name: "ItensEstoque");

            migrationBuilder.DropTable(
                name: "ItensMovimentacao");

            migrationBuilder.DropTable(
                name: "KardexDiario");

            migrationBuilder.DropTable(
                name: "Estoques");

            migrationBuilder.DropTable(
                name: "Itens");

            migrationBuilder.DropTable(
                name: "Movimentacoes");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Unidades");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "TiposMovimentacoes");

            migrationBuilder.DropTable(
                name: "Setor");
        }
    }
}
