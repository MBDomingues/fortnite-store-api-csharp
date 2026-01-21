using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fortinite_Project.Web.Migrations
{
    /// <inheritdoc />
    public partial class ModelagemFinalCorrigida : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cosmeticos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", nullable: false),
                    Raridade = table.Column<string>(type: "TEXT", nullable: false),
                    UrlImagem = table.Column<string>(type: "TEXT", nullable: false),
                    Preco = table.Column<int>(type: "INTEGER", nullable: false),
                    isForSale = table.Column<bool>(type: "INTEGER", nullable: false),
                    dataInclusao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    isBundle = table.Column<bool>(type: "INTEGER", nullable: false),
                    bundleItemsJson = table.Column<string>(type: "TEXT", nullable: false),
                    coresJson = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cosmeticos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Senha = table.Column<string>(type: "TEXT", nullable: false),
                    creditos = table.Column<int>(type: "INTEGER", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HistoricoTransacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UsuarioId = table.Column<int>(type: "INTEGER", nullable: true),
                    CosmeticoId = table.Column<string>(type: "TEXT", nullable: true),
                    TipoTransacao = table.Column<string>(type: "TEXT", nullable: true),
                    Valor = table.Column<int>(type: "INTEGER", nullable: false),
                    DataTransacao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoTransacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricoTransacoes_Cosmeticos_CosmeticoId",
                        column: x => x.CosmeticoId,
                        principalTable: "Cosmeticos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HistoricoTransacoes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ItensAdquiridos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UsuarioId = table.Column<int>(type: "INTEGER", nullable: true),
                    DataCompra = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensAdquiridos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensAdquiridos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CosmeticoItemAdquirido",
                columns: table => new
                {
                    CosmeticoId = table.Column<string>(type: "TEXT", nullable: false),
                    ItemAdquiridoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CosmeticoItemAdquirido", x => new { x.CosmeticoId, x.ItemAdquiridoId });
                    table.ForeignKey(
                        name: "FK_CosmeticoItemAdquirido_Cosmeticos_CosmeticoId",
                        column: x => x.CosmeticoId,
                        principalTable: "Cosmeticos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CosmeticoItemAdquirido_ItensAdquiridos_ItemAdquiridoId",
                        column: x => x.ItemAdquiridoId,
                        principalTable: "ItensAdquiridos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CosmeticoItemAdquirido_ItemAdquiridoId",
                table: "CosmeticoItemAdquirido",
                column: "ItemAdquiridoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoTransacoes_CosmeticoId",
                table: "HistoricoTransacoes",
                column: "CosmeticoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoTransacoes_UsuarioId",
                table: "HistoricoTransacoes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensAdquiridos_UsuarioId",
                table: "ItensAdquiridos",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CosmeticoItemAdquirido");

            migrationBuilder.DropTable(
                name: "HistoricoTransacoes");

            migrationBuilder.DropTable(
                name: "ItensAdquiridos");

            migrationBuilder.DropTable(
                name: "Cosmeticos");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
