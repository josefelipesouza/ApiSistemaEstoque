using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiSistemaEstoque.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateTransporte : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transportes",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlacaVeiculo = table.Column<string>(type: "TEXT", nullable: false),
                    CodigoMovimentacao = table.Column<int>(type: "INTEGER", nullable: false),
                    CodigoItem = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantidade = table.Column<int>(type: "INTEGER", nullable: false),
                    DataEntrada = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataSaida = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transportes", x => x.Codigo);
                    table.ForeignKey(
                        name: "FK_Transportes_Itens_CodigoItem",
                        column: x => x.CodigoItem,
                        principalTable: "Itens",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transportes_Movimentacoes_CodigoMovimentacao",
                        column: x => x.CodigoMovimentacao,
                        principalTable: "Movimentacoes",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transportes_CodigoItem",
                table: "Transportes",
                column: "CodigoItem");

            migrationBuilder.CreateIndex(
                name: "IX_Transportes_CodigoMovimentacao",
                table: "Transportes",
                column: "CodigoMovimentacao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transportes");
        }
    }
}
